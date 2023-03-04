using System;
using System.Collections.Generic;
using Game.Code.Logic.ResourcesLogic;

namespace Game.Code.Data.Game
{
    [Serializable]
    public class GameProgress
    {
        public ResourceStorageData ResourceStorageData;

        public GameProgress()
        {
            ResourceStorageData = new ResourceStorageData();
        }
    }

    public class ResourceStorageData
    {
        public Action<ResourceType> ResourceChanged;
        
        public readonly Dictionary<ResourceType, int> StoredAmountResourceByType;

        public ResourceStorageData()
        {
            StoredAmountResourceByType = new Dictionary<ResourceType, int>
            {
                [ResourceType.Wood] = 0,
                [ResourceType.Stone] = 0,
                [ResourceType.Coal] = 0,
                [ResourceType.Food] = 0,
            };
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            StoredAmountResourceByType[resourceType] += amount;
            ResourceChanged?.Invoke(resourceType);
        }
        
        public void GetResource(ResourceType resourceType, int amount)
        {
            StoredAmountResourceByType[resourceType] -= amount;
            ResourceChanged?.Invoke(resourceType);
        }
    }
}