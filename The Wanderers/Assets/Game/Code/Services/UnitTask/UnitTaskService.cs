using System;
using System.Collections;
using System.Collections.Generic;
using Game.Code.Commander;
using Game.Code.Common;
using Game.Code.Core;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Units;
using UnityEngine;

namespace Game.Code.Services.UnitTask
{
    public class UnitTaskService : IUnitTaskService
    {
        public event Action NotifyUnit;
        
        private readonly DynamicGameContext _dynamicGameContext;
        private readonly ICoroutineRunner _coroutineRunner;

        private List<Unit> _allUnits;
        private Queue<UnitTask> _currentTaskForUnits;

        // тестовые деревья под сруб
        private List<ResourceNodeSpawner> _chopTrees;

        private WaitForSeconds _notifyDelay;
        private bool _notifyRoutineIsRunning;
        
        public UnitTaskService(List<ResourceNodeSpawner> nodeSpawners, DynamicGameContext dynamicGameContext, ICoroutineRunner coroutineRunner)
        {
            _dynamicGameContext = dynamicGameContext;
            _coroutineRunner = coroutineRunner;

            _allUnits = new List<Unit>();
            _chopTrees = new List<ResourceNodeSpawner>();
            _currentTaskForUnits = new Queue<UnitTask>();
            
            // для теста
            foreach (var resourceNodeSpawner in nodeSpawners) 
                _chopTrees.Add(resourceNodeSpawner);

            _notifyDelay = new WaitForSeconds(0.7f);
        }

        public bool HasTask() => 
            _currentTaskForUnits.Count > 0;

        public UnitTask GetTask(Unit unit)
        {
            //Debug.Log($"Задачу получил {unit.name}");
            unit.TaskCompleted += OnUnitTaskCompleted;
            
            UnitTask task = _currentTaskForUnits.Dequeue();
            return task;
        }

        // юнит может прервать задачу и тогда он должен ее вернуть в список
        public void AddTask(UnitTask task) => 
            _currentTaskForUnits.Enqueue(task);

        public void ClearAllTask() =>
            _currentTaskForUnits.Clear();
        
        public void CreateGatherResourceTask(Resource resource, Storage storage)
        {
            if (resource.IsAvailable())
            {
                GrabResourceCommand command = new GrabResourceCommand()
                {
                    Resource = resource,
                    Target = resource.transform,
                    Goal = storage.GetInteractionPoint(),
                    Storage = storage
                };
                
                AddTask(new UnitTask(command));
            }
            
            NotifyAllAvailableUnits();
        }

        public void CreateChopTreeTask(Storage storage)
        {
            foreach (ResourceNodeSpawner nodeSpawner in _chopTrees)
            {
                if (nodeSpawner.HasResource())
                {
                    ChopTreeCommand chopTreeCommand = new ChopTreeCommand()
                    {
                        ResourceNode = nodeSpawner.GetResourceNode(),
                        Target = nodeSpawner.GetResourceNode().transform,
                        Goal = storage.transform
                    };

                    AddTask(new UnitTask(command: chopTreeCommand));
                }
            }
            
            NotifyAllAvailableUnits();
        }

        private void OnUnitTaskCompleted(Unit unit, UnitTask task)
        {
            //Debug.Log($"Задачу сдал {unit.name}, команда: {task.GetCommand()}, STATUS: {task.GetTaskStatus()}");
            
            // может сломаться??
            if (task.GetTaskStatus() == TaskStatus.Failed)
                AddTask(task);
            
            unit.TaskCompleted -= OnUnitTaskCompleted;
        }

        private void NotifyAllAvailableUnits()
        {
            if (_notifyRoutineIsRunning == false)
                _coroutineRunner.StartCoroutine(NotifyUnitsAboutTaskRoutine());
        }

        private IEnumerator NotifyUnitsAboutTaskRoutine()
        {
            _notifyRoutineIsRunning = true;
            
            while (_currentTaskForUnits.Count > 0)
            {
                NotifyUnit?.Invoke();
                yield return _notifyDelay;
            }

            _notifyRoutineIsRunning = false;
        }
    }

    public class UnitTask
    {
        private readonly ICommand _unitCommand;

        private TaskStatus _taskStatus;

        public UnitTask(ICommand command, TaskStatus taskStatus = TaskStatus.Running)
        {
            _unitCommand = command;
            _taskStatus = taskStatus;
        }

        public ICommand GetCommand() => _unitCommand;

        public TaskStatus GetTaskStatus() => _taskStatus;
        public void SetTaskStatus(TaskStatus status) => _taskStatus = status;
    }
}