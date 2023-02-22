using System.Collections.Generic;
using Game.Code.Data.StaticData.ResourceNodeData;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Infrastructure.Factories
{
    public class GameFactory : IResourceMiningFactory
    {
        private readonly StaticDataService _staticDataService;

        public GameFactory(StaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public List<ResourceNodeSpawner> CreateNodeSpawners()
        {
            List<ResourceNodeSpawner> nodeSpawners = new List<ResourceNodeSpawner>();

            ResourceSpawnersData spawnersOnLevel = _staticDataService.GetResourceSpawnersData();

            foreach (SpawnerData spawnerData in spawnersOnLevel.ResourceSpawners)
            {
                ResourceNodeData nodeData = _staticDataService.GetDataForNode(spawnerData.Type);
                ResourceNodeSpawner spawner = 
                    Object.Instantiate(spawnersOnLevel.Prefab, spawnerData.Position, Quaternion.identity);
                
                spawner.transform.SetParent(spawnerData.Container);
                spawner.Init(spawnerData.Type, nodeData, this);
                nodeSpawners.Add(spawner);
            }

            return nodeSpawners;
        }

        public ResourceNode CreateResourceNode(ResourceType type, Transform at)
        {
            ResourceNodeData nodeData = _staticDataService.GetDataForNode(type);
            ResourceNode node = Object.Instantiate(nodeData.Prefab, at.position, Quaternion.identity);
            node.transform.SetParent(at);
            node.Init(nodeData);
            
            return node;
        }

        public Resource CreateResource(ResourceType type, Vector3 at)
        {
            ResourceData resourceData = _staticDataService.GetDataForResource(type);
            
            Resource resource = Object.Instantiate(resourceData.Prefab, at, Quaternion.identity);
            resource.Init(resourceData.Type);
            resource.name = $"Resource: {type.ToString()}, Position: {resource.transform.position.x}";
            
            return resource;
        }
    }
}