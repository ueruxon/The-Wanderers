using System;
using System.Collections;
using Game.Code.Infrastructure.Factories;
using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.UI.Elements;
using UnityEngine;

namespace Game.Code.Logic.Buildings.ProductionBuildings
{
    public class Quarry : Building, IProductionBuilding
    {
        public event Action<Resource> ResourceSpawned;

        [SerializeField] private Transform _spawnResourcePoint;
        [SerializeField] private ResourceType _productionResource;
        [SerializeField] private ProgressBar _progressBar;

        private IResourceMiningFactory _factory;
        private Villager _worker;
        
        private ProductionState _productionState;

        private float _timeToSpawnResource = 10f;
        private float _currentTimer = 0f;

        private bool _showProgress;
        private WaitForSeconds _updateDelay;

        public void Init(IResourceMiningFactory factory)
        {
            _factory = factory;
            _productionState = ProductionState.Inactive;
            
            _progressBar.Hide();
            _showProgress = false;
            _updateDelay = new WaitForSeconds(0.2f);

            //StartCoroutine(UpdateProgressBarRoutine());
        }

        public bool IsAvailable() => 
            _productionState == ProductionState.Inactive;

        public void PrepareForWork(Villager villager)
        {
            _worker = villager;
            _productionState = ProductionState.PrepareForWork;
        }

        public bool IsReserved(Villager villager) => 
            _worker == villager;

        public void ClearWorkspace()
        {
            _worker = null;
            _productionState = ProductionState.Inactive;
            
            _showProgress = false;
            _progressBar.Hide();
        }

        public void Interaction()
        {
            _currentTimer += Time.deltaTime;
            
            if (_currentTimer > _timeToSpawnResource)
            {
                _currentTimer = 0f;
                SpawnResource();
            }

            UpdateProgressBar();
        }

        // private IEnumerator UpdateProgressBarRoutine()
        // {
        //     // пока здание стоит?
        //     while (true)
        //     {
        //         yield return _updateDelay;
        //         UpdateProgressBar();
        //     }
        // }
        
        private void UpdateProgressBar()
        {
            if (_showProgress == false && _currentTimer > 0.5f)
            {
                _showProgress = true;
                _progressBar.Show();
            }

            if (_showProgress && _currentTimer < 0.5f)
            {
                _showProgress = false;
                _progressBar.Hide();
            }
            
            if (_showProgress) 
                _progressBar.SetValue(_currentTimer, _timeToSpawnResource);
        }

        private void SpawnResource()
        {
            Resource resource = _factory.CreateResource(_productionResource, _spawnResourcePoint.position);
            ResourceSpawned?.Invoke(resource);
        }
    }
}