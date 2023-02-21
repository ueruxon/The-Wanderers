using System;
using System.Collections.Generic;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Data.StaticData.ResourceNodeData
{
    [CreateAssetMenu(fileName = "ResourceSpawners", menuName = "Resources/ResourceSpawners", order = 0)]
    public class ResourceSpawnersData : ScriptableObject
    {
        public List<SpawnerData> ResourceSpawners;
    }
    
    [Serializable]
    public class SpawnerData
    {
        public ResourceType Type;
        public Vector3 Position;
        public ResourceNodeSpawner Spawner;

        public SpawnerData(ResourceNodeSpawner spawner, ResourceType type, Vector3 spawnerPosition)
        {
            Spawner = spawner;
            Type = type;
            Position = spawnerPosition;
        }
    }
}