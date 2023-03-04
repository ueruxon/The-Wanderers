using System.Collections.Generic;
using Game.Code.Data.Game;
using Game.Code.Infrastructure.Services.Progress;

namespace Game.Code.Logic.ResourcesLogic
{
    public class ResourceRepository
    {
        private readonly IGameProgressService _progressService;
        
        private ResourceStorageData _resourceStorageData;
        private Dictionary<ResourceType, int> _reservedResources;

        public ResourceRepository(IGameProgressService progressService)
        {
            _progressService = progressService;
            _reservedResources = new Dictionary<ResourceType, int>();
        }

        public void Init() => 
            _resourceStorageData = _progressService.Progress.ResourceStorageData;

        public void AddResource(ResourceType resourceType, int amount) => 
            _resourceStorageData.AddResource(resourceType, amount);
    }
}