using System.Collections.Generic;
using Game.Code.Data.StaticData.ResourceNodeData;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Logic.ResourcesLogic;

namespace Game.Code.Infrastructure.Factories
{
    public class GameFactory
    {
        private readonly StaticDataService _staticDataService;

        public GameFactory(StaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public List<ResourceNodeSpawner> CreateNodeSpawners()
        {
            List<ResourceNodeSpawner> spawners = _staticDataService.GetNodeSpawners();

            foreach (ResourceNodeSpawner spawner in spawners)
            {
                ResourceNodeData nodeData = _staticDataService.GetDataForNode(spawner.GetResourceType());
                ResourceData resourceData = _staticDataService.GetDataForResource(spawner.GetResourceType());
                
                spawner.Init(nodeData, resourceData);
            }

            return spawners;
        }
    }
}