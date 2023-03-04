using Game.Code.Infrastructure.Services.AssetManagement;
using Game.Code.Infrastructure.Services.Progress;
using Game.Code.Infrastructure.Services.StaticData;
using Game.Code.Logic.Selection;
using Game.Code.UI.Elements;
using UnityEngine;

namespace Game.Code.UI.Services.Factory
{
    public class UIFactory
    {
        private readonly AssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IGameProgressService _progressService;
        private readonly SelectionHandler _selectionHandler;

        private Transform _uiRoot;

        public UIFactory(AssetProvider assetProvider,
            IStaticDataService staticDataService, 
            IGameProgressService progressService,
            SelectionHandler selectionHandler)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _progressService = progressService;
            _selectionHandler = selectionHandler;
        }
        
        public void CreateUIRoot() => 
            _uiRoot = _assetProvider.Instantiate<GameObject>(AssetPath.UIRootPath).transform;

        public void CreateHUD()
        {
           GameObject hud = _assetProvider.Instantiate<GameObject>(AssetPath.UIHudPath);
           hud.transform.SetParent(_uiRoot);

           // добавить окно на hud, со всеми ссылками на внутренние компоненты, когда их будет больше 
           foreach (var resourceCounter in hud.GetComponentsInChildren<ResourceCounter>())
               resourceCounter.Init(_progressService);

           foreach (var button in hud.GetComponentsInChildren<SelectionButton>())
               button.Init(_selectionHandler);
        }
    }
}