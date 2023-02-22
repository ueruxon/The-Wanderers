using System.Collections.Generic;
using System.Linq;
using Game.Code.Data.StaticData.ResourceNodeData;
using Game.Code.Infrastructure.Services.AssetManagement;
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
    }
}