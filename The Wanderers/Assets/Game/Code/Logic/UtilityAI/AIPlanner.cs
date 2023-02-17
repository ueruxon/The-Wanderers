using System;
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
        private IUnitTaskService _taskService;
        private AIContext _aiContext;
        private Actor _currentActor;
        
        private GlobalActorTask _currentTask;
        public GlobalActorTask CurrentTask => _currentTask;

        private IdleCommand _baseIdleCommand;
        private GlobalActorTask _baseIdleTask;
        
        public void Init(Actor actor, DynamicGameContext dynamicGameContext, AIContext aiContext, IUnitTaskService taskService)
        {
            _currentActor = actor;
            _gameContext = dynamicGameContext;
            _aiContext = aiContext;
            _taskService = taskService;
            _taskService.NotifyUnit += OnNotifyUnitAboutTask;

            _baseIdleCommand = new IdleCommand{ Target = _currentActor.transform };
            _baseIdleTask = new GlobalActorTask(_baseIdleCommand);
            _currentTask = _baseIdleTask;
            
            _aiContext.SetActionCommand(_baseIdleCommand);
        }

        private void OnNotifyUnitAboutTask()
        {
            if (_currentActor.IsAvailable()) 
                TryExecuteTask();
        }

        private bool TryExecuteTask()
        {
            if (_taskService.HasTask())
            {
                _currentTask = _taskService.GetTask(_currentActor);
                _aiContext.SetActionCommand(_currentTask.GetCommand());
            
                TaskReceived?.Invoke(_aiContext);
                return true;
            }

            return false;
        }

        public void CompleteCurrentTask()
        {
            _currentTask.SetTaskStatus(TaskStatus.Completed);
            TaskCompleted?.Invoke(_currentTask);

            // пробуем взять новую задачу
            if (TryExecuteTask() == false)
            {
                _currentTask = _baseIdleTask;
                _aiContext.SetActionCommand(_baseIdleCommand);
                _aiContext.MoveTarget = _baseIdleCommand.Target;
            }
        }

        public void Cleanup() => 
            _taskService.NotifyUnit -= OnNotifyUnitAboutTask;
    }

    
}