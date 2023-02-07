using System.Collections.Generic;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Units;

namespace Game.Code.Core
{
    // глобальный контекст игры, который содержит данные о сессии
    public class DynamicGameContext
    {
        private readonly Dictionary<ResourceType, List<Storage>> _storagesByType;
        private readonly Dictionary<ResourceType, Queue<Resource>> _minedResourcesByType;

        private readonly Queue<Unit> _homelessUnits;

        public DynamicGameContext()
        {
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
            
            _homelessUnits = new Queue<Unit>();
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

        public void AddHomelessUnit(Unit unit) =>
            _homelessUnits.Enqueue(unit);
        
        public int GetHomelessCount() => 
            _homelessUnits.Count;
        
        public Unit GetHomelessUnit() => 
            _homelessUnits.Dequeue();
    }
}