using System;
using System.Collections.Generic;
using Game.Code.Commander;
using Game.Code.Common;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Units;
using Game.Code.Services.UnitTask;
using UnityEngine;

namespace Game.Code.Core
{
    public class GameLoop : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private Storage _storage;
        [SerializeField] private Unit _unitPrefab;

        [SerializeField] private List<ResourceNodeSpawner> _resourceSpawners;
        [SerializeField] private int _unitsCount = 3;
    
        private DynamicGameContext _dynamicGameContext;
        private UnitTaskService _taskService;

        
        // для теста
        [SerializeField] private UnitCommandType _command;
        private UnitCommandType _currentCommand;
        
        private void Awake()
        {
            // глобальный контекст должен быть доступен из любого места? Сервис?
            _dynamicGameContext = new DynamicGameContext(_resourceSpawners, _storage);
            _taskService = new UnitTaskService(_resourceSpawners, _dynamicGameContext, this);
        
            foreach (ResourceNodeSpawner nodeSpawner in _resourceSpawners) 
                nodeSpawner.Init(_dynamicGameContext);

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
            
            // для теста
            _dynamicGameContext.ResourceReceived += OnResourceReceived;
        }

        private void OnResourceReceived(Resource resource)
        {
            // для теста
            _taskService.CreateGatherResourceTask(resource);
        }

        private void Update()
        {
            // для теста
            if (_command != _currentCommand)
            {
                SetUnitCommand(_command);
            }
        }

        private void SetUnitCommand(UnitCommandType command)
        {
            _currentCommand = command;
            _taskService.AcceptGlobalUnitCommand(_currentCommand);
        }
    }
}
