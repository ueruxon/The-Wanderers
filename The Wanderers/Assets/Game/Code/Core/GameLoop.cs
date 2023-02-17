using System.Collections.Generic;
using Game.Code.Common;
using Game.Code.Infrastructure.Services.UnitTask;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.Camera;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Selection;
using Game.Code.Logic.Units;
using Game.Code.Logic.UtilityAI.Commander;
using UnityEngine;

namespace Game.Code.Core
{
    public class GameLoop : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private SelectionHandler _selectionHandler;

        [SerializeField] private Actor _actorPrefab;
        [SerializeField] private int _unitsCount = 3;
        [Space(8)]


        [SerializeField] private House _housePrefab;
        [SerializeField] private List<House> _testHouses;
        [Space(8)]

        [SerializeField] private Storage _storagePrefab;
        [SerializeField] private List<Storage> _storages;
        [Space(8)]

        [SerializeField] private List<ResourceNodeSpawner> _resourceSpawners;

        private DynamicGameContext _dynamicGameContext;
        private ActorTaskService _taskService;
        
        private void Awake()
        {
            _cameraController.Init();
            _selectionHandler.Init();
            
            // глобальный контекст должен быть доступен из любого места? Сервис?
            _dynamicGameContext = new DynamicGameContext();
            _taskService = new ActorTaskService(_selectionHandler, _dynamicGameContext, this);

            foreach (ResourceNodeSpawner nodeSpawner in _resourceSpawners)
            {
                nodeSpawner.Init();
                nodeSpawner.ResourceSpawned += OnResourceSpawned;
            }

            foreach (Storage storage in _storages)
            {
                storage.Init();
                _dynamicGameContext.AddStorageInList(storage);
            }

            for (int i = 0; i < _unitsCount; i++)
            {
                Vector3 spawnPoint = new Vector3(
                    transform.position.x + i, 
                    transform.position.y, 
                    transform.position.z + i);
                
                Actor actor = Instantiate(_actorPrefab, spawnPoint, Quaternion.identity);
                actor.Init(_dynamicGameContext, _taskService);
                actor.name = $"Actor: {i + 1}";
                
                // должна делать фабрика
                _dynamicGameContext.AddHomelessUnit(actor);
            }

            
            // для теста домов
            foreach (House house in _testHouses)
            {
                house.Init();
                OnHouseBuilt(house);
            }
        }
        
        private void Update()
        {
            // для теста
            if (Input.GetKeyDown(KeyCode.K))
            {
                Vector3 pos = new Vector3(57, 0, 20);
                Storage storage = Instantiate(_storagePrefab, pos, Quaternion.identity);
                storage.Init();
                // ивент создания??
                OnStorageBuilt(storage);
            }
            
            // для теста
            if (Input.GetKeyDown(KeyCode.L))
            {
                Vector3 pos = new Vector3(34, 0, 37);
                Storage storage = Instantiate(_storagePrefab, pos, Quaternion.identity);
                storage.Init();
                // ивент создания??
                OnStorageBuilt(storage);
            }
            
            // для теста
            if (Input.GetKeyDown(KeyCode.J))
            {
                Vector3 pos = new Vector3(25.48f, 0, -25);
                House house = Instantiate(_housePrefab, pos, Quaternion.identity);
                house.Init();
                // ивент создания??
                OnHouseBuilt(house);
            }
        }
        
        private void OnResourceSpawned(Resource resource)
        {
            if (TryAddResourceInStorage(resource) == false)
                _dynamicGameContext.AddMinedResourceInQueue(resource);
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
                    Actor homeless = _dynamicGameContext.GetHomelessUnit();

                    if (house.CanRegisterUnit())
                    {
                        house.RegisterUnit(homeless);
                        homeless.RegisterHome(house);
                        continue;
                    }
                    
                    _dynamicGameContext.AddHomelessUnit(homeless);
                }
            }
        }
        
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
