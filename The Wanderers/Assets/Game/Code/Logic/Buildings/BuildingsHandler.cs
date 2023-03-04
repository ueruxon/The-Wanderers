using System.Collections.Generic;
using Game.Code.Infrastructure.Context;
using Game.Code.Infrastructure.Factories;
using Game.Code.Infrastructure.Services.ActorTask;
using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.Buildings.ProductionBuildings;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Logic.Buildings
{
    public class BuildingsHandler
    {
        private readonly GameFactory _gameFactory;
        private readonly IActorTaskService _taskService;
        private readonly DynamicGameContext _gameContext;
        private readonly ResourceRepository _resourceRepository;
        private readonly List<Quarry> _testQuarryList;
        private readonly List<Storage> _testStorages;
        private readonly List<House> _testHouses;

        public BuildingsHandler(GameFactory gameFactory,
            IActorTaskService taskService,
            DynamicGameContext gameContext,
            ResourceRepository resourceRepository,
            List<Quarry> testQuarryList,
            List<Storage> testStorage,
            List<House> testHouses)
        {
            _gameFactory = gameFactory;
            _taskService = taskService;
            _gameContext = gameContext;
            _resourceRepository = resourceRepository;
            _testQuarryList = testQuarryList;
            _testStorages = testStorage;
            _testHouses = testHouses;
        }

        // для теста
        public void Init()
        {
            foreach (Quarry quarry in _testQuarryList)
            {
                quarry.Init(_gameFactory);
                OnProductionBuildingBuilt(quarry);
            }

            foreach (Storage storage in _testStorages)
            {
                storage.Init(_resourceRepository);
                OnStorageBuilt(storage);
            }

            foreach (House house in _testHouses)
            {
                house.Init();
                OnHouseBuilt(house);
            }
        }

        // тип здания?? 
        private void OnBuildingBuilt()
        {
            
        }
        
        private void OnStorageBuilt(Storage storage)
        {
            _gameContext.AddStorageInList(storage);

            ResourceType resourceType = storage.GetStoredResourceType();
            
            if (_gameContext.HasResourceByTypeInQueue(resourceType))
            {
                int queueCount = _gameContext.GetResourceCountInQueue(resourceType);
                
                for (int i = 0; i < queueCount; i++)
                {
                    Resource resource = _gameContext.GetResourceByTypeInQueue(resourceType);

                    if (TryAddResourceInStorage(resource) == false)
                    {
                        _gameContext.AddMinedResourceInQueue(resource);
                        break;
                    }
                }
            }
        }
        
        private void OnHouseBuilt(House house)
        {
            if (_gameContext.GetHomelessCount() > 0)
            {
                int homelessUnitCount = _gameContext.GetHomelessCount();

                for (int i = 0; i < homelessUnitCount; i++)
                {
                    Villager homeless = _gameContext.GetHomelessVillager();

                    if (house.CanRegisterUnit())
                    {
                        house.RegisterUnit(homeless);
                        homeless.RegisterHome(house);
                        continue;
                    }
                    
                    _gameContext.AddHomelessVillager(homeless);
                }
            }
        }

        private void OnProductionBuildingBuilt(IProductionBuilding productionBuilding)
        {
            _gameContext.AddProductionBuilding(productionBuilding);
            productionBuilding.ResourceSpawned += OnResourceSpawned;
        }

        private void OnResourceSpawned(Resource resource)
        {
            if (TryAddResourceInStorage(resource) == false) 
                _gameContext.AddMinedResourceInQueue(resource);
        }
        
        private bool TryAddResourceInStorage(Resource resource)
        {
            List<Storage> storages = _gameContext.GetStoragesByType(resource.GetResourceType());

            foreach (Storage storage in storages)
            {
                if (storage.IsFull() == false && storage.HasPlaceForReserve())
                {
                    storage.ReservePlaceForResource();
                    _taskService.CreateGatherResourceTask(resource, storage);
                    
                    return true;
                }
            }

            return false;
        }
    }
}