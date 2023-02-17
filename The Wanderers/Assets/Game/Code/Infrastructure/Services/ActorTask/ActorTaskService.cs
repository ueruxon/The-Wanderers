using System;
using System.Collections;
using System.Collections.Generic;
using Game.Code.Common;
using Game.Code.Core;
using Game.Code.Infrastructure.Factories;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Selection;
using Game.Code.Logic.Units;
using Game.Code.Logic.UtilityAI.Commander;
using UnityEngine;

namespace Game.Code.Infrastructure.Services.UnitTask
{
    public class ActorTaskService : IUnitTaskService
    {
        public event Action NotifyUnit;

        private readonly SelectionHandler _selectionHandler;
        private readonly DynamicGameContext _dynamicGameContext;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ActorTaskFactory _taskFactory;

        private List<Actor> _allUnits;
        private Queue<GlobalActorTask> _globalTaskQueue;
        
        private WaitForSeconds _notifyDelay;
        private bool _notifyRoutineIsRunning;
        
        public ActorTaskService(SelectionHandler selectionHandler, DynamicGameContext dynamicGameContext, ICoroutineRunner coroutineRunner)
        {
            _selectionHandler = selectionHandler;
            _selectionHandler.ResourceNodeSelected += OnMiningCommand;
            
            _dynamicGameContext = dynamicGameContext;
            _coroutineRunner = coroutineRunner;
            _taskFactory = new ActorTaskFactory();

            _allUnits = new List<Actor>();
            _globalTaskQueue = new Queue<GlobalActorTask>();
            

            _notifyDelay = new WaitForSeconds(0.7f);
        }

        private void OnMiningCommand(SelectionMode mode)
        {
            if (mode == SelectionMode.Select)
            {
                List<ResourceNode> selectedNodes = _selectionHandler.GetSelectedNodes();
                
                foreach (ResourceNode resourceNode in selectedNodes)
                {
                    resourceNode.Prepare(true);
                    
                    GlobalActorTask task = _taskFactory.CreateMiningTask(resourceNode);
                    AddTask(task);
                }
            }
            
            NotifyAllAvailableUnits();
        }

        public bool HasTask() => 
            _globalTaskQueue.Count > 0;

        public GlobalActorTask GetTask(Actor actor)
        {
            //Debug.Log($"Задачу получил {unit.name}");
            actor.TaskCompleted += OnUnitTaskCompleted;
            
            GlobalActorTask task = _globalTaskQueue.Dequeue();
            return task;
        }

        // юнит может прервать задачу и тогда он должен ее вернуть в список
        public void AddTask(GlobalActorTask task) => 
            _globalTaskQueue.Enqueue(task);

        public void ClearAllTask() =>
            _globalTaskQueue.Clear();
        
        public void CreateGatherResourceTask(Resource resource, Storage storage)
        {
            if (resource.IsAvailable())
            {
                GlobalActorTask task = _taskFactory.CreateGatherResourceTask(resource, storage);
                AddTask(task);
                
                NotifyAllAvailableUnits();
            }
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
            
            while (_globalTaskQueue.Count > 0)
            {
                NotifyUnit?.Invoke();
                yield return _notifyDelay;
            }

            _notifyRoutineIsRunning = false;
        }

        public void Cleanup()
        {
            _selectionHandler.ResourceNodeSelected -= OnMiningCommand;
        }
    }
}