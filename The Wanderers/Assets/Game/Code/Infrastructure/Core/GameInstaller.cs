using Game.Code.Common;
using Game.Code.Data;
using Game.Code.Infrastructure.Factories;
using Game.Code.Infrastructure.Services.AssetManagement;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Infrastructure.Services.UnitTask;
using Game.Code.Logic.Camera;
using Game.Code.Logic.Game;
using Game.Code.Logic.Selection;

namespace Game.Code.Infrastructure.Core
{
    public class GameInstaller
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly SelectionHandler _selectionHandler;
        private readonly CameraController _cameraController;


        private GameInitializer _gameInitializer;
        private GameLoop _gameLoop;

        public GameInstaller(ICoroutineRunner gameRunner, SelectionHandler selectionHandler,
            CameraController cameraController)
        {
            _coroutineRunner = gameRunner;
            _selectionHandler = selectionHandler;
            _cameraController = cameraController;

            InstallSystems();
        }

        private void InstallSystems()
        {
            AssetProvider assetProvider = new AssetProvider();
            StaticDataService staticDataService = new StaticDataService(assetProvider);
            ActorTaskService actorTaskService = new ActorTaskService(_selectionHandler, _coroutineRunner);
            GameFactory gameFactory = new GameFactory(staticDataService);
            
            DynamicGameContext dynamicGameContext = new DynamicGameContext();

            _gameInitializer = new GameInitializer(_selectionHandler, _cameraController, staticDataService, gameFactory);
        }
    }
}