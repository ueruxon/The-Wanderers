using System;
using System.Collections.Generic;
using Game.Code.Common;
using Game.Code.Data;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.Buildings.ProductionBuildings;
using Game.Code.Logic.Camera;
using Game.Code.Logic.Selection;
using UnityEngine;

namespace Game.Code.Infrastructure.Core
{
    public class GameRunner : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private SelectionHandler _selectionHandler;
        [SerializeField] private CameraController _cameraController;

        [SerializeField] private List<Quarry> _testQuarryList;
        [SerializeField] private List<Storage> _testStorages;
        [SerializeField] private List<House> _testHouses;

        private GameInstaller _gameInstaller;

        private void Awake()
        {
            _gameInstaller = 
                new GameInstaller(
                    _gameConfig, 
                    this, 
                    _selectionHandler,
                    _cameraController, 
                    _testQuarryList, 
                    _testStorages,
                    _testHouses);
        }
    }
}