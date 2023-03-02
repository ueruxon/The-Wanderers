using System.Collections.Generic;
using Game.Code.Infrastructure.Context;
using Game.Code.Infrastructure.Services.ActorTask;
using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Infrastructure.Core
{
    public class GameLoop
    {
        private House _housePrefab;
        private List<House> _testHouses;

        private Storage _storagePrefab;
        private List<Storage> _storages;

        private DynamicGameContext _dynamicGameContext;
        private ActorTaskService _taskService;

        private void Awake()
        {
            // foreach (Storage storage in _storages)
            // {
            //     storage.Init();
            //     _dynamicGameContext.AddStorageInList(storage);
            // }
            //
            // //для теста домов
            // foreach (House house in _testHouses)
            // {
            //     house.Init();
            //     OnHouseBuilt(house);
            // }
        }

        private void Update()
        {
            // для теста
            if (Input.GetKeyDown(KeyCode.K))
            {
                Vector3 pos = new Vector3(57, 0, 20);
                Storage storage = Object.Instantiate(_storagePrefab, pos, Quaternion.identity);
                storage.Init();
                // ивент создания??
                OnStorageBuilt(storage);
            }
            
            // для теста
            if (Input.GetKeyDown(KeyCode.L))
            {
                Vector3 pos = new Vector3(34, 0, 37);
                Storage storage = Object.Instantiate(_storagePrefab, pos, Quaternion.identity);
                storage.Init();
                // ивент создания??
                OnStorageBuilt(storage);
            }
            
            // для теста
            if (Input.GetKeyDown(KeyCode.J))
            {
                Vector3 pos = new Vector3(25.48f, 0, -25);
                House house = Object.Instantiate(_housePrefab, pos, Quaternion.identity);
                house.Init();
                // ивент создания??
                OnHouseBuilt(house);
            }
        }
        
        // для теста (вынести в другой класс)
        private void OnStorageBuilt(Storage storage)
        {
            _dynamicGameContext.AddStorageInList(storage);

            ResourceType resourceType = storage.GetStoredResourceType();
            
            if (_dynamicGameContext.HasResourceByTypeInQueue(resourceType))
            {
                int queueCount = _dynamicGameContext.GetResourceCountInQueue(resourceType);
                
                for (int i = 0; i < queueCount; i++)
                {
                    Resource resource = _dynamicGameContext.GetResourceByTypeInQueue(resourceType);

                    if (TryAddResourceInStorage(resource) == false)
                    {
                        _dynamicGameContext.AddMinedResourceInQueue(resource);
                        break;
                    }
                }
            }
        }

        // для теста (вынести в другой класс для строительства)
        private void OnHouseBuilt(House house)
        {
            if (_dynamicGameContext.GetHomelessCount() > 0)
            {
                int homelessUnitCount = _dynamicGameContext.GetHomelessCount();

                for (int i = 0; i < homelessUnitCount; i++)
                {
                    Villager homeless = _dynamicGameContext.GetHomelessVillager();

                    if (house.CanRegisterUnit())
                    {
                        house.RegisterUnit(homeless);
                        homeless.RegisterHome(house);
                        continue;
                    }
                    
                    _dynamicGameContext.AddHomelessVillager(homeless);
                }
            }
        }
        
        // можем ли поместить ресурс в хранилище
        // и если можем, то создаем задачу
        private bool TryAddResourceInStorage(Resource resource)
        {
            List<Storage> storages = _dynamicGameContext.GetStoragesByType(resource.GetResourceType());

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
