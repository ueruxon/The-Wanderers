using System;
using Game.Code.Logic.UtilityAI.Commander;

namespace Game.Code.Infrastructure.Services.UnitTask
{
    public class GlobalActorTask
    {
        public event Action Canceled;
        
        private string _taskID;
        private readonly ICommand _actorCommand;

        private TaskStatus _taskStatus;

        public GlobalActorTask(ICommand command, string taskID = "",TaskStatus taskStatus = TaskStatus.Running)
        {
            _actorCommand = command;
            _taskStatus = taskStatus;
            _taskID = taskID;
        }

        public ICommand GetCommand() => 
            _actorCommand;

        public TaskStatus GetTaskStatus() => 
            _taskStatus;
        
        public void SetTaskStatus(TaskStatus status)
        {
            _taskStatus = status;
            switch (_taskStatus)
            {
                case TaskStatus.Running:
                    break;
                case TaskStatus.Failed:
                    break;
                case TaskStatus.Completed:
                    break;
                case TaskStatus.Canceled:
                    Canceled?.Invoke();
                    break;
            }
        }

        public string GetID() => 
            _taskID;
    }
}