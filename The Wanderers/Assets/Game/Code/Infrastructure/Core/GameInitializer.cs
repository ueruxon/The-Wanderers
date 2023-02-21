using System.Collections.Generic;
using Game.Code.Data.StaticData.ResourceNodeData;
using Game.Code.Infrastructure.Factories;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Logic.Camera;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Selection;

namespace Game.Code.Infrastructure.Core
{
    public class GameInitializer
    {
        private readonly SelectionHandler _selectionHandler;
        private readonly CameraController _cameraController;
        private readonly StaticDataService _staticDataService;
        private readonly GameFactory _gameFactory;


        public GameInitializer(SelectionHandler selectionHandler,
            CameraController cameraController,
            StaticDataService staticDataService, 
            GameFactory gameFactory)
        {
            _selectionHandler = selectionHandler;
            _cameraController = cameraController;
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;

            InitializeSystems();
            InitGameWorld();
        }

        private void InitializeSystems()
        {
            _selectionHandler.Init();
            _cameraController.Init();
            _staticDataService.Init();
        }

        private void InitGameWorld()
        {
            InitNodeSpawners();
        }

        private void InitNodeSpawners()
        {
            _gameFactory.CreateNodeSpawners();
        }
    }
}