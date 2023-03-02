using System;
using Game.Code.Infrastructure.Factories;
using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Logic.Buildings.ProductionBuildings
{
    public class Quarry : Building, IProductionBuilding
    {
        public event Action<Resource> ResourceSpawned;
        
        [SerializeField] private ResourceType _productionResource;
        [SerializeField] private Transform _spawnResourcePoint;

        private IResourceMiningFactory _factory;
        public Villager _worker;
        
        private ProductionState _productionState;

        private float _timeToSpawnResource = 10f;
        private float _currentTimer = 0f;

        public void Init(IResourceMiningFactory factory)
        {
            _factory = factory;
            _productionState = ProductionState.Inactive;
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
        }

        public void Interaction()
        {
            _currentTimer += Time.deltaTime;
            
            if (_currentTimer > _timeToSpawnResource)
            {
                _currentTimer = 0f;
                
                SpawnResource();
            }
        }

        private void SpawnResource()
        {
            Resource resource = _factory.CreateResource(_productionResource, _spawnResourcePoint.position);
            ResourceSpawned?.Invoke(resource);
        }
    }
}