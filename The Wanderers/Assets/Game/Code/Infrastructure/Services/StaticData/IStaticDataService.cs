using Game.Code.Data.StaticData.Actors;
using Game.Code.Data.StaticData.ResourceNode;
using Game.Code.Logic.Actors;
using Game.Code.Logic.ResourcesLogic;

namespace Game.Code.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        public void Init();
        public ResourceSpawnersData GetResourceSpawnersData();
        public ResourceNodeData GetDataForNode(ResourceType type);
        public ResourceData GetDataForResource(ResourceType type);
        public ActorData GetDataForActor(ActorType type);
    }
}