using System.Collections.Generic;
using Game.Code.Infrastructure.Context;
using Game.Code.Infrastructure.Factories;
using Game.Code.Infrastructure.Services.ActorTask;
using Game.Code.Logic.Buildings;

namespace Game.Code.Logic.ResourcesLogic
{
    public class ResourceMiningController
    {
        private readonly DynamicGameContext _dynamicGameContext;
        private readonly GameFactory _gameFactory;
        private readonly ActorTaskService _taskService;

        private List<ResourceNodeSpawner> _spawners;

        public ResourceMiningController(DynamicGameContext dynamicGameContext, 
            GameFactory gameFactory, 
            ActorTaskService taskService)
        {
            _dynamicGameContext = dynamicGameContext;
            _gameFactory = gameFactory;
            _taskService = taskService;
        }

        public void InitNodeSpawners()
        {
            _spawners = _gameFactory.CreateNodeSpawners();

            foreach (ResourceNodeSpawner nodeSpawner in _spawners) 
                nodeSpawner.ResourceSpawned += OnResourceSpawned;
        }

        private void OnResourceSpawned(Resource resource)
        {
            if (TryAddResourceInStorage(resource) == false)
                _dynamicGameContext.AddMinedResourceInQueue(resource);
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

        public void Cleanup()
        {
            foreach (ResourceNodeSpawner nodeSpawner in _spawners) 
                nodeSpawner.ResourceSpawned -= OnResourceSpawned;
        }
    }
}