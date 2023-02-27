using System;
using System.Collections;
using System.Collections.Generic;
using Game.Code.Common;
using Game.Code.Infrastructure.Core;
using Game.Code.Infrastructure.Factories;
using Game.Code.Logic.Actors;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.Game;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Selection;
using UnityEngine;

namespace Game.Code.Infrastructure.Services.UnitTask
{
    public class ActorTaskService : IActorTaskService
    {
        public event Action NotifyActor;

        private readonly SelectionHandler _selectionHandler;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ActorTaskFactory _taskFactory;
        
        private TaskPool _taskPool;
        
        private WaitForSeconds _notifyDelay;
        private bool _notifyRoutineIsRunning;
        
        public ActorTaskService(SelectionHandler selectionHandler, ICoroutineRunner coroutineRunner)
        {
            _taskFactory = new ActorTaskFactory();
            _taskPool = new TaskPool();
            _notifyDelay = new WaitForSeconds(0.5f);

            _selectionHandler = selectionHandler;
            _selectionHandler.ResourceNodeSelected += OnMiningCommand;
            
            _coroutineRunner = coroutineRunner;
        }

        public bool HasTask() => 
            _taskPool.HasTask();

        public GlobalActorTask GetTask(Actor actor)
        {
            actor.TaskCompleted += OnUnitTaskCompleted;
            
            GlobalActorTask actorTask = _taskPool.GetAvailableTask();
            return actorTask;
        }

        // юнит может прервать задачу и тогда он должен ее вернуть в список
        public void AddTask(GlobalActorTask task) => 
            _taskPool.AddTask(task);
        
        public void CreateGatherResourceTask(Resource resource, Storage storage)
        {
            if (resource.IsAvailable())
            {
                GlobalActorTask task = _taskFactory.CreateGatherResourceTask(resource, storage);
                _taskPool.AddTask(task);
                
                NotifyAllAvailableUnits();
            }
        }

        private void OnMiningCommand(SelectionMode mode)
        {
            List<ResourceNode> selectedNodes = _selectionHandler.GetSelectedNodes();
                
            foreach (ResourceNode resourceNode in selectedNodes)
            {
                if (mode == SelectionMode.Select)
                {
                    if (_taskPool.Contains(resourceNode.ID) == false)
                    {
                        resourceNode.PrepareForWork(true);
                        
                        GlobalActorTask task = _taskFactory.CreateMiningTask(resourceNode);
                        _taskPool.AddTask(task);
                    }
                }

                if (mode == SelectionMode.Deselect)
                {
                    resourceNode.PrepareForWork(false);
                    _taskPool.CancelTask(resourceNode.ID);
                }
            }

            NotifyAllAvailableUnits();
        }

        private void OnUnitTaskCompleted(Actor actor, GlobalActorTask task)
        {
            // может сломаться??
            if (task.GetTaskStatus() == TaskStatus.Failed)
                _taskPool.AddTask(task);
            
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
            
            while (_taskPool.HasTask())
            {
                NotifyActor?.Invoke();
                yield return _notifyDelay;
            }

            _notifyRoutineIsRunning = false;
        }

        public void Cleanup()
        {
            _selectionHandler.ResourceNodeSelected -= OnMiningCommand;
        }
    }

    public class TaskPool
    {
        private Queue<GlobalActorTask> _availableTasksQueue;
        private List<GlobalActorTask> _tasksInWork;
        
        public TaskPool()
        {
            _availableTasksQueue = new Queue<GlobalActorTask>();
            _tasksInWork = new List<GlobalActorTask>();
        }

        public void AddTask(GlobalActorTask task) => 
            _availableTasksQueue.Enqueue(task);

        public GlobalActorTask GetAvailableTask()
        {
            GlobalActorTask task = _availableTasksQueue.Dequeue();
            _tasksInWork.Add(task);
            
            return task;
        }
        
        public bool HasTask() => 
            _availableTasksQueue.Count > 0;

        // пересчет коллекции. Не производительно?
        public void CancelTask(string taskID)
        {
            int taskQueueCount = _availableTasksQueue.Count;

            for (int i = 0; i < taskQueueCount; i++)
            {
                GlobalActorTask task = _availableTasksQueue.Dequeue();
                
                if (task.GetID() != taskID) 
                    AddTask(task);
            }
            
            _tasksInWork.RemoveAll(task =>
            {
                if (task.GetID() == taskID)
                {
                    task.SetTaskStatus(TaskStatus.Canceled);
                    return true;
                }

                return false;
            });
        }


        // пересчет коллекции. Не производительно?
        public bool Contains(string taskID)
        {
            int taskCount = _availableTasksQueue.Count;
            
            for (int i = 0; i < taskCount; i++)
            {
                GlobalActorTask task = _availableTasksQueue.Dequeue();
                
                if (task.GetID() == taskID)
                    return true;

                AddTask(task);
            }
            
            return false;
        }
    }
}