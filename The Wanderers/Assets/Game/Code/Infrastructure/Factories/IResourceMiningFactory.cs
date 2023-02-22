using System.Collections.Generic;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Infrastructure.Factories
{
    public interface IResourceMiningFactory
    {
        public List<ResourceNodeSpawner> CreateNodeSpawners();
        ResourceNode CreateResourceNode(ResourceType type, Transform at);
        Resource CreateResource(ResourceType resourceType, Vector3 at);
    }
}