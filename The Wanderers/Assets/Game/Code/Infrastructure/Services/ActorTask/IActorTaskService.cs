using System;
using Game.Code.Logic.Actors;

namespace Game.Code.Infrastructure.Services.UnitTask
{
    public interface IActorTaskService : IService
    {
        public event Action NotifyActor;
        public bool HasTask();
        public GlobalActorTask GetTask(Actor actor);
        public void AddTask(GlobalActorTask task);
    }
}