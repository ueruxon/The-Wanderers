﻿using System;
using Game.Code.Core;
using Game.Code.Infrastructure.Services.UnitTask;
using Game.Code.Logic.Units;
using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI
{
    [RequireComponent(typeof(AIBrain))]
    public class AIPlanner : MonoBehaviour
    {
        public event Action<AIContext> TaskReceived;
        public event Action<GlobalActorTask> TaskCompleted;

        private DynamicGameContext _gameContext;
        private IActorTaskService _taskService;
        private AIContext _aiContext;
        private Actor _currentActor;

        private GlobalActorTask _currentTask;
        public GlobalActorTask CurrentTask => _currentTask;

        private IdleCommand _baseIdleCommand;
        private GlobalActorTask _baseIdleTask;

        public void Init(Actor actor, DynamicGameContext dynamicGameContext, AIContext aiContext,
            IActorTaskService taskService)
        {
            _currentActor = actor;
            _gameContext = dynamicGameContext;
            _aiContext = aiContext;
            _taskService = taskService;
            _taskService.NotifyActor += OnNotifyUnitAboutTask;

            _baseIdleCommand = new IdleCommand {Target = _currentActor.transform};
            _baseIdleTask = new GlobalActorTask(_baseIdleCommand);
            _currentTask = _baseIdleTask;

            _aiContext.SetActionCommand(_baseIdleCommand);
        }

        public void CompleteGlobalTask()
        {
            _currentTask.SetTaskStatus(TaskStatus.Completed);
            _currentTask.Canceled -= CompleteGlobalTask;

            TaskCompleted?.Invoke(_currentTask);

            // пробуем взять новую задачу
            if (_taskService.HasTask())
            {
                ExecuteTask();
                return;
            }
            
            _currentTask = _baseIdleTask;
            _aiContext.SetActionCommand(_baseIdleCommand);
            _aiContext.MoveTarget = _baseIdleCommand.Target;
        }

        private void OnNotifyUnitAboutTask()
        {
            if (_currentActor.IsAvailable() && _taskService.HasTask())
                ExecuteTask();
        }

        private void ExecuteTask()
        {
            _currentTask = _taskService.GetTask(_currentActor);
            _currentTask.Canceled += CompleteGlobalTask;
            
            _aiContext.SetActionCommand(_currentTask.GetCommand());

            TaskReceived?.Invoke(_aiContext);
        }

        public void Cleanup() =>
            _taskService.NotifyActor -= OnNotifyUnitAboutTask;
    }
}