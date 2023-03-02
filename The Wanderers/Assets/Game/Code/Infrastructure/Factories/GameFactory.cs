using System.Collections.Generic;
using Game.Code.Common;
using Game.Code.Data.StaticData.Actors;
using Game.Code.Data.StaticData.ResourceNode;
using Game.Code.Infrastructure.Context;
using Game.Code.Infrastructure.Services.ActorTask;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Logic.Actors;
using Game.Code.Logic.ResourcesLogic;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Code.Infrastructure.Factories
{
    public class GameFactory : IResourceMiningFactory
    {
        private readonly StaticDataService _staticDataService;
        private readonly DynamicGameContext _dynamicGameContext;
        private readonly IActorTaskService _actorTaskService;

        public GameFactory(StaticDataService staticDataService, 
            DynamicGameContext dynamicGameContext, 
            IActorTaskService actorTaskService)
        {
            _staticDataService = staticDataService;
            _dynamicGameContext = dynamicGameContext;
            _actorTaskService = actorTaskService;
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

        public T CreateActor<T>(ActorType actorType, GameObject actorsContainer, Vector3 at) where T : Actor
        {
            ActorData actorData = _staticDataService.GetDataForActor(actorType);
            T actor = Object.Instantiate(actorData.Prefab, at, quaternion.identity) as T;
            actor.Construct(actorData, _dynamicGameContext, _actorTaskService);
            actor.name = $"{actorType.ToString()} + {UniqueIDGenerator.GenerateID(actor.gameObject)}";
            actor.transform.SetParent(actorsContainer.transform);

            return actor;
        }
    }
}