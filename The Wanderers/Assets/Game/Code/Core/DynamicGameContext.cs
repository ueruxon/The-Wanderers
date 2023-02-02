using System;
using System.Collections.Generic;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Core
{
    // глобальный контекст игры, который содержит данные о сессии
    public class DynamicGameContext
    {
        public event Action<Resource> ResourceReceived;
        
        private readonly List<ResourceNodeSpawner> _resourceSpawners;
        private readonly Storage _storage;
        
        private List<Resource> _resourcesInWorld;
        
        public DynamicGameContext(List<ResourceNodeSpawner> resourceSpawners, Storage storage)
        {
            _resourceSpawners = resourceSpawners;
            _storage = storage;

            _resourcesInWorld = new List<Resource>();
        }
        
        public void AddResource(Resource resource)
        {
            _resourcesInWorld.Add(resource);
            
            ResourceReceived?.Invoke(resource);
        }

        public List<Resource> GetResourcesInWorld() => 
            _resourcesInWorld;

        public void RemoveResource(Resource resource)
        {
            if (_resourcesInWorld.Contains(resource)) 
                _resourcesInWorld.Remove(resource);
        }

        public Storage GetStorage() => 
            _storage;

        public List<ResourceNodeSpawner> GetResourceNodeSpawners() => 
            _resourceSpawners;
    }
}