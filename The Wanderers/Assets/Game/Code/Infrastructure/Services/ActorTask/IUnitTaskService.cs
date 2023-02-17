using System;
using Game.Code.Logic.Units;

namespace Game.Code.Infrastructure.Services.UnitTask
{
    public interface IUnitTaskService : IService
    {
        public event Action NotifyUnit;
        public bool HasTask();
        public GlobalActorTask GetTask(Actor actor);
        public void AddTask(GlobalActorTask task);
    }
}