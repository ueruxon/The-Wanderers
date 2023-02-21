using System.Collections.Generic;
using System.Linq;
using Game.Code.Data.StaticData.ResourceNodeData;
using Game.Code.Infrastructure.Services.AssetManagement;
using Game.Code.Logic.ResourcesLogic;

namespace Game.Code.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private readonly AssetProvider _assetProvider;

        private List<ResourceNodeSpawner> _nodeSpawners;
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
            _nodeSpawners = _assetProvider.Load<ResourceSpawnersData>(AssetPath.ResourceSpawnerPath)
                .ResourceSpawners
                .Select(x => x.Spawner)
                .ToList();
            
            _resourceNodeDataByType = _assetProvider.LoadAll<ResourceNodeData>(AssetPath.ResourceNodeDataPath)
                .ToDictionary(x => x.Type, x => x);
            
            _resourceDataByType = _assetProvider.LoadAll<ResourceData>(AssetPath.ResourceDataPath)
                .ToDictionary(x => x.Type, x => x);
        }

        public List<ResourceNodeSpawner> GetNodeSpawners() => 
            _nodeSpawners;

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