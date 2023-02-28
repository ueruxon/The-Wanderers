using System.Collections.Generic;
using System.Linq;
using Game.Code.Data.StaticData.Actors;
using Game.Code.Data.StaticData.ResourceNode;
using Game.Code.Infrastructure.Services.AssetManagement;
using Game.Code.Logic.Actors;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly AssetProvider _assetProvider;
        
        private ResourceSpawnersData _resourceSpawnersData;
        private Dictionary<ResourceType, ResourceNodeData> _resourceNodeDataByType;
        private Dictionary<ResourceType, ResourceData> _resourceDataByType;

        private Dictionary<ActorType, ActorData> _actorDataByType;

        public StaticDataService(AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void Init() => 
            LoadData();

        private void LoadData()
        {
            _resourceSpawnersData = _assetProvider.Load<ResourceSpawnersData>(AssetPath.ResourceSpawnerDataPath);
            _resourceNodeDataByType = _assetProvider.LoadAll<ResourceNodeData>(AssetPath.ResourceNodeDataPath)
                .ToDictionary(x => x.Type, x => x);
            _resourceDataByType = _assetProvider.LoadAll<ResourceData>(AssetPath.ResourceDataPath)
                .ToDictionary(x => x.Type, x => x);
            
            _actorDataByType = _assetProvider.LoadAll<ActorData>(AssetPath.ActorsDataPath)
                .ToDictionary(x => x.Type, x => x);
        }
        
        public ResourceSpawnersData GetResourceSpawnersData() => 
            _resourceSpawnersData;

        public ResourceNodeData GetDataForNode(ResourceType type)
        {
            return _resourceNodeDataByType.TryGetValue(type, out ResourceNodeData data)
                ? data
                : null;
        }
        
        public ResourceData GetDataForResource(ResourceType type)
        {
            return _resourceDataByType.TryGetValue(type, out ResourceData data)
                ? data
                : null;
        }

        public ActorData GetDataForActor(ActorType type)
        {
            return _actorDataByType.TryGetValue(type, out ActorData data)
                ? data
                : null;
        }
    }
}