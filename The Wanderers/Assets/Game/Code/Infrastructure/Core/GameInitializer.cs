using Game.Code.Data;
using Game.Code.Infrastructure.Factories;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Logic.Camera;
using Game.Code.Logic.Game;
using Game.Code.Logic.Selection;

namespace Game.Code.Infrastructure.Core
{
    public class GameInitializer
    {
        private readonly SelectionHandler _selectionHandler;
        private readonly CameraController _cameraController;
        private readonly StaticDataService _staticDataService;
        private readonly ResourceMiningController _resourceMiningController;
        private readonly ActorSpawner _actorSpawner;


        public GameInitializer(SelectionHandler selectionHandler,
            CameraController cameraController,
            StaticDataService staticDataService,
            ResourceMiningController resourceMiningController, 
            ActorSpawner actorSpawner)
        {
            _selectionHandler = selectionHandler;
            _cameraController = cameraController;
            _staticDataService = staticDataService;
            _resourceMiningController = resourceMiningController;
            _actorSpawner = actorSpawner;

            InitializeSystems();
            InitGameWorld();
        }

        private void InitializeSystems()
        {
            _staticDataService.Init();
            _selectionHandler.Init();
            _cameraController.Init();
            _actorSpawner.Init();
        }

        private void InitGameWorld()
        {
            _resourceMiningController.InitNodeSpawners();
            _actorSpawner.InitActors();
        }
        
    }
}