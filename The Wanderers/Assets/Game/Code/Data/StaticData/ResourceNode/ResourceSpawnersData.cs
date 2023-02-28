using System;
using System.Collections.Generic;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Data.StaticData.ResourceNode
{
    [CreateAssetMenu(fileName = "ResourceSpawners", menuName = "Resources/ResourceSpawners", order = 0)]
    public class ResourceSpawnersData : ScriptableObject
    {
        public ResourceNodeSpawner Prefab;
        public List<SpawnerData> ResourceSpawners;
    }
    
    [Serializable]
    public class SpawnerData
    {
        public ResourceType Type;
        public Vector3 Position;
        public Transform Container;

        public SpawnerData(ResourceType type, Vector3 spawnerPosition, Transform container)
        {
            Type = type;
            Position = spawnerPosition;
            Container = container;
        }
    }
}