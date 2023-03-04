using System.Collections.Generic;
using Game.Code.Common;
using Game.Code.Data;
using Game.Code.Infrastructure.Context;
using Game.Code.Infrastructure.Factories;
using Game.Code.Infrastructure.Services.ActorTask;
using Game.Code.Infrastructure.Services.AssetManagement;
using Game.Code.Infrastructure.Services.Progress;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Logic.Actors;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.Buildings.ProductionBuildings;
using Game.Code.Logic.Camera;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Selection;
using Game.Code.UI.Services.Factory;

namespace Game.Code.Infrastructure.Core
{
    public class GameInstaller
    {
        private readonly GameConfig _gameConfig;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly SelectionHandler _selectionHandler;
        private readonly CameraController _cameraController;
        
        private readonly List<Quarry> _testQuarryList;
        private readonly List<Storage> _testStorages;
        private readonly List<House> _testHouses;

        private GameInitializer _gameInitializer;
        private GameLoop _gameLoop;

        public GameInstaller(GameConfig gameConfig, 
            ICoroutineRunner gameRunner, 
            SelectionHandler selectionHandler,
            CameraController cameraController, 
            List<Quarry> testQuarryList, 
            List<Storage> testStorages,
            List<House> testHouses)
        {
            _gameConfig = gameConfig;
            _coroutineRunner = gameRunner;
            _selectionHandler = selectionHandler;
            _cameraController = cameraController;
            _testQuarryList = testQuarryList;
            _testStorages = testStorages;
            _testHouses = testHouses;

            InstallSystems();
        }

        private void InstallSystems()
        {
            AssetProvider assetProvider = new AssetProvider();
            IStaticDataService staticDataService = new StaticDataService(assetProvider);
            IGameProgressService progressService = new GameProgressService();
            DynamicGameContext dynamicGameContext = new DynamicGameContext();
            
            ActorTaskService actorTaskService = new ActorTaskService(_selectionHandler, _coroutineRunner);
            GameFactory gameFactory = new GameFactory(staticDataService, dynamicGameContext, actorTaskService);
            UIFactory uiFactory = new UIFactory(assetProvider, staticDataService, progressService, _selectionHandler);
            ResourceMiningController miningController = new ResourceMiningController(dynamicGameContext, gameFactory, actorTaskService);
            ActorSpawner actorSpawner = new ActorSpawner(_gameConfig, gameFactory, dynamicGameContext);
            ResourceRepository resourceRepository = new ResourceRepository(progressService);
            BuildingsHandler buildingsHandler = new BuildingsHandler(gameFactory, 
                actorTaskService, dynamicGameContext, resourceRepository, _testQuarryList, _testStorages, _testHouses); 

            _gameInitializer = new GameInitializer(
                _selectionHandler, 
                _cameraController,
                progressService,
                staticDataService,
                uiFactory,
                miningController, 
                resourceRepository,
                actorSpawner,
                buildingsHandler);
        }
    }
}