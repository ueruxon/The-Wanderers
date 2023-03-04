using Game.Code.Data;
using Game.Code.Data.Game;
using Game.Code.Infrastructure.Factories;
using Game.Code.Infrastructure.Services.Progress;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Logic.Actors;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.Camera;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Selection;
using Game.Code.UI.Services.Factory;

namespace Game.Code.Infrastructure.Core
{
    public class GameInitializer
    {
        private readonly SelectionHandler _selectionHandler;
        private readonly CameraController _cameraController;
        private readonly IGameProgressService _gameProgressService;
        private readonly IStaticDataService _staticDataService;
        private readonly UIFactory _uiFactory;
        private readonly ResourceMiningController _resourceMiningController;
        private readonly ResourceRepository _resourceRepository;
        private readonly ActorSpawner _actorSpawner;
        private readonly BuildingsHandler _buildingsHandler;
        
        public GameInitializer(SelectionHandler selectionHandler,
            CameraController cameraController,
            IGameProgressService gameProgressService,
            IStaticDataService staticDataService,
            UIFactory uiFactory,
            ResourceMiningController miningController,
            ResourceRepository resourceRepository,
            ActorSpawner actorSpawner,
            BuildingsHandler buildingsHandler)
        {
            _selectionHandler = selectionHandler;
            _cameraController = cameraController;
            _gameProgressService = gameProgressService;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _resourceMiningController = miningController;
            _resourceRepository = resourceRepository;
            _actorSpawner = actorSpawner;
            _buildingsHandler = buildingsHandler;

            LoadProgressOrInitNew();
            InitializeSystems();
            InitUI();
            InitGameWorld();
        }

        private void InitializeSystems()
        {
            _staticDataService.Init();
            _selectionHandler.Init();
            _cameraController.Init();
            _actorSpawner.Init();
            _resourceRepository.Init();
        }

        // async
        private void LoadProgressOrInitNew() => 
            _gameProgressService.Progress = new GameProgress();

        private void InitUI()
        {
            _uiFactory.CreateUIRoot();
            _uiFactory.CreateHUD();
        }

        // async
        private void InitGameWorld()
        {
            _resourceMiningController.InitNodeSpawners();
            _actorSpawner.InitActors();
            _buildingsHandler.Init();
        }
        
    }
}