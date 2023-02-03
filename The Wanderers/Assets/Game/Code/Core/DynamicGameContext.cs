using System.Collections.Generic;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;

namespace Game.Code.Core
{
    // глобальный контекст игры, который содержит данные о сессии
    public class DynamicGameContext
    {
        private readonly List<ResourceNodeSpawner> _resourceSpawners;
        private readonly Dictionary<ResourceType, List<Storage>> _storagesByType;
        private readonly Dictionary<ResourceType, Queue<Resource>> _minedResourcesByType;

        public DynamicGameContext(List<ResourceNodeSpawner> resourceSpawners)
        {
            _resourceSpawners = resourceSpawners;

            _minedResourcesByType = new Dictionary<ResourceType, Queue<Resource>>
            {
                [ResourceType.Food] = new(),
                [ResourceType.Stone] = new(),
                [ResourceType.Wood] = new(),
                [ResourceType.Coal] = new()
            };

            _storagesByType = new Dictionary<ResourceType, List<Storage>>
            {
                [ResourceType.Food] = new(),
                [ResourceType.Stone] = new(),
                [ResourceType.Wood] = new(),
                [ResourceType.Coal] = new()
            };
        }
        
        public void AddMinedResourceInQueue(Resource resource) => 
            _minedResourcesByType[resource.GetResourceType()].Enqueue(resource);

        public bool HasResourceByTypeInQueue(ResourceType type) => 
            _minedResourcesByType[type].Count > 0;
        
        public Resource GetResourceByTypeInQueue(ResourceType type) => 
            _minedResourcesByType[type].Dequeue();

        public int GetResourceCountInQueue(ResourceType resourceType) =>
            _minedResourcesByType[resourceType].Count;

        public void AddStorageInList(Storage storage) => 
            _storagesByType[storage.GetStoredResourceType()].Add(storage);
        
        public List<Storage> GetStoragesByType(ResourceType type) => 
            _storagesByType[type];
    }
}