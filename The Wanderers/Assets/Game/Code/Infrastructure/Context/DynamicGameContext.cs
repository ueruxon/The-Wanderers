using System.Collections.Generic;
using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.Buildings.ProductionBuildings;
using Game.Code.Logic.ResourcesLogic;

namespace Game.Code.Infrastructure.Context
{
    // глобальный контекст игры, который содержит данные о сессии
    public class DynamicGameContext
    {
        private readonly Dictionary<ResourceType, List<Storage>> _storagesByType;
        private readonly Dictionary<ResourceType, Queue<Resource>> _minedResourcesByType;

        private readonly Queue<Villager> _homelessVillagers;
        private readonly List<Villager> _villagerActors;
        private readonly List<IProductionBuilding> _productionBuildings;

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
            
            _homelessVillagers = new Queue<Villager>();
            _villagerActors = new List<Villager>();
            _productionBuildings = new List<IProductionBuilding>();
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

        public void AddHomelessVillager(Villager villager) =>
            _homelessVillagers.Enqueue(villager);
        
        public int GetHomelessCount() => 
            _homelessVillagers.Count;
        
        public Villager GetHomelessVillager() => 
            _homelessVillagers.Dequeue();

        public void AddVillager(Villager villager) => 
            _villagerActors.Add(villager);

        public List<Villager> GetVillagers() => 
            _villagerActors;

        public void AddProductionBuilding(IProductionBuilding productionBuilding) => 
            _productionBuildings.Add(productionBuilding);

        public List<IProductionBuilding> GetProductionBuildings() => 
            _productionBuildings;
    }
}