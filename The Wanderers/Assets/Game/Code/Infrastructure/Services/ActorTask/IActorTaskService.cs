using System;
using Game.Code.Logic.Actors;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;

namespace Game.Code.Infrastructure.Services.ActorTask
{
    public interface IActorTaskService : IService
    {
        public event Action NotifyActor;
        public bool HasTask();
        public GlobalActorTask GetTask(Actor actor);
        public void AddTask(GlobalActorTask task);
        public void CreateGatherResourceTask(Resource resource, Storage storage);
    }
}