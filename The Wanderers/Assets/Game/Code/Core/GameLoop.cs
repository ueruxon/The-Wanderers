using System;
using System.Collections.Generic;
using Game.Code.Commander;
using Game.Code.Common;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.Camera;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Units;
using Game.Code.Services.UnitTask;
using UnityEngine;

namespace Game.Code.Core
{
    public class GameLoop : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private CameraController _cameraController;
        
        [SerializeField] private Storage _storage;
        [SerializeField] private Unit _unitPrefab;

        
        [SerializeField] private Storage _storagePrefab;
        [SerializeField] private List<Storage> _storages;
        [SerializeField] private List<ResourceNodeSpawner> _resourceSpawners;
        [SerializeField] private int _unitsCount = 3;
    
        private DynamicGameContext _dynamicGameContext;
        private UnitTaskService _taskService;

        
        // для теста
        [SerializeField] private UnitCommandType _command;
        private UnitCommandType _currentCommand;
        
        private void Awake()
        {
            _cameraController.Init();
            
            // глобальный контекст должен быть доступен из любого места? Сервис?
            _dynamicGameContext = new DynamicGameContext(_resourceSpawners);
            _taskService = new UnitTaskService(_resourceSpawners, _dynamicGameContext, this);

            foreach (ResourceNodeSpawner nodeSpawner in _resourceSpawners)
            {
                nodeSpawner.Init();
                nodeSpawner.ResourceSpawned += OnResourceSpawned;
            }

            foreach (Storage storage in _storages) 
                _dynamicGameContext.AddStorageInList(storage);

            for (int i = 0; i < _unitsCount; i++)
            {
                Vector3 spawnPoint = new Vector3(
                    transform.position.x + i, 
                    transform.position.y, 
                    transform.position.z + i);
                
                Unit unit = Instantiate(_unitPrefab, spawnPoint, Quaternion.identity);
                unit.Init(_dynamicGameContext, _taskService);
                unit.name = $"Unit {i + 1}";
            }
        }
        
        private void Update()
        {
            // для теста
            if (_command != _currentCommand)
            {
                SetUnitCommand(_command);
            }

            // для теста
            if (Input.GetKeyDown(KeyCode.K))
            {
                Vector3 pos = new Vector3(57, 0, 20);
                Storage storage = Instantiate(_storagePrefab, pos, Quaternion.identity);
                // ивент создания??
                OnStorageBuilt(storage);
            }
            
            // для теста
            if (Input.GetKeyDown(KeyCode.L))
            {
                Vector3 pos = new Vector3(34, 0, 37);
                Storage storage = Instantiate(_storagePrefab, pos, Quaternion.identity);
                // ивент создания??
                OnStorageBuilt(storage);
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

        private void SetUnitCommand(UnitCommandType command)
        {
            _currentCommand = command;

            if (_currentCommand == UnitCommandType.ChopTree)
            {
                _taskService.CreateChopTreeTask(_storages[0]);
            }
        }
    }
}
