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
        private readonly GameFactory _gameFactory;
        private readonly ResourceMiningController _resourceMiningController;


        public GameInitializer(SelectionHandler selectionHandler,
            CameraController cameraController,
            StaticDataService staticDataService,
            GameFactory gameFactory, 
            ResourceMiningController resourceMiningController)
        {
            _selectionHandler = selectionHandler;
            _cameraController = cameraController;
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
            _resourceMiningController = resourceMiningController;

            InitializeSystems();
            InitGameWorld();
        }

        private void InitializeSystems()
        {
            _staticDataService.Init();
            _selectionHandler.Init();
            _cameraController.Init();
        }

        private void InitGameWorld()
        {
            _resourceMiningController.InitNodeSpawners();
        }
        
    }
}