using System;
using System.Collections;
using System.Collections.Generic;
using Game.Code.Common;
using Game.Code.Core;
using Game.Code.Infrastructure.Factories;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Units;
using Game.Code.Logic.UtilityAI.Commander;
using UnityEngine;

namespace Game.Code.Infrastructure.Services.UnitTask
{
    public class ActorTaskService : IUnitTaskService
    {
        public event Action NotifyUnit;
        
        private readonly DynamicGameContext _dynamicGameContext;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ActorTaskFactory _taskFactory;

        private List<Actor> _allUnits;
        private Queue<GlobalActorTask> _currentTaskForUnits;

        // тестовые деревья под сруб
        private List<ResourceNodeSpawner> _chopTrees;

        private WaitForSeconds _notifyDelay;
        private bool _notifyRoutineIsRunning;
        
        public ActorTaskService(List<ResourceNodeSpawner> nodeSpawners, DynamicGameContext dynamicGameContext, ICoroutineRunner coroutineRunner)
        {
            _dynamicGameContext = dynamicGameContext;
            _coroutineRunner = coroutineRunner;
            _taskFactory = new ActorTaskFactory();

            _allUnits = new List<Actor>();
            _chopTrees = new List<ResourceNodeSpawner>();
            _currentTaskForUnits = new Queue<GlobalActorTask>();
            
            // для теста
            foreach (var resourceNodeSpawner in nodeSpawners) 
                _chopTrees.Add(resourceNodeSpawner);

            _notifyDelay = new WaitForSeconds(0.7f);
        }

        public bool HasTask() => 
            _currentTaskForUnits.Count > 0;

        public GlobalActorTask GetTask(Actor actor)
        {
            //Debug.Log($"Задачу получил {unit.name}");
            actor.TaskCompleted += OnUnitTaskCompleted;
            
            GlobalActorTask task = _currentTaskForUnits.Dequeue();
            return task;
        }

        // юнит может прервать задачу и тогда он должен ее вернуть в список
        public void AddTask(GlobalActorTask task) => 
            _currentTaskForUnits.Enqueue(task);

        public void ClearAllTask() =>
            _currentTaskForUnits.Clear();
        
        public void CreateGatherResourceTask(Resource resource, Storage storage)
        {
            if (resource.IsAvailable())
            {
                GlobalActorTask task = _taskFactory.CreateGatherResourceTask(resource, storage);
                AddTask(task);
                
                NotifyAllAvailableUnits();
            }
        }

        public void CreateChopTreeTask()
        {
            foreach (ResourceNodeSpawner nodeSpawner in _chopTrees)
            {
                if (nodeSpawner.HasResource())
                {
                    ResourceNode node = nodeSpawner.GetResourceNode();
                    node.Prepare(true);
                    
                    GlobalActorTask task = _taskFactory.CreateMiningTask(node);
                    AddTask(task);
                }
            }
            
            NotifyAllAvailableUnits();
        }

        private void OnUnitTaskCompleted(Actor actor, GlobalActorTask task)
        {
            // может сломаться??
            if (task.GetTaskStatus() == TaskStatus.Failed)
                AddTask(task);
            
            actor.TaskCompleted -= OnUnitTaskCompleted;
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
}