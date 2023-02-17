using Game.Code.Logic.UtilityAI.Commander;

namespace Game.Code.Infrastructure.Services.UnitTask
{
    public class GlobalActorTask
    {
        private readonly ICommand _actorCommand;

        private TaskStatus _taskStatus;

        public GlobalActorTask(ICommand command, TaskStatus taskStatus = TaskStatus.Running)
        {
            _actorCommand = command;
            _taskStatus = taskStatus;
        }

        public ICommand GetCommand() => _actorCommand;

        public TaskStatus GetTaskStatus() => _taskStatus;
        public void SetTaskStatus(TaskStatus status) => _taskStatus = status;
    }
}