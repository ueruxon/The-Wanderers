using System;
using Game.Code.Core;
using Game.Code.Logic.Units;
using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;
using Game.Code.Services.UnitTask;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI
{
    [RequireComponent(typeof(AIBrain))]
    public class AIPlanner : MonoBehaviour
    {
        public event Action<AIContext> TaskReceived;
        public event Action<UnitTask> TaskCompleted;

        private DynamicGameContext _gameContext;
        private IUnitTaskService _taskService;
        private AIContext _aiContext;
        private Unit _currentUnit;
        
        private UnitTask _currentTask;
        public UnitTask CurrentTask => _currentTask;

        private IdleCommand _baseIdleCommand;
        private UnitTask _baseIdleTask;
        
        public void Init(Unit unit, DynamicGameContext dynamicGameContext, AIContext aiContext, IUnitTaskService taskService)
        {
            _currentUnit = unit;
            _gameContext = dynamicGameContext;
            _aiContext = aiContext;
            _taskService = taskService;
            _taskService.NotifyUnit += OnNotifyUnitAboutTask;

            _baseIdleCommand = new IdleCommand{ Target = _currentUnit.transform };
            _baseIdleTask = new UnitTask(_baseIdleCommand);
            _currentTask = _baseIdleTask;
            
            _aiContext.SetActionCommand(_baseIdleCommand);
        }

        private void OnNotifyUnitAboutTask()
        {
            if (_currentUnit.IsAvailable()) 
                TryExecuteTask();
        }

        private bool TryExecuteTask()
        {
            if (_taskService.HasTask())
            {
                _currentTask = _taskService.GetTask(_currentUnit);
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