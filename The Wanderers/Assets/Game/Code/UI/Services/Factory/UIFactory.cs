using Game.Code.Infrastructure.Services.AssetManagement;
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
        private readonly SelectionHandler _selectionHandler;

        private Transform _uiRoot;

        public UIFactory(AssetProvider assetProvider, 
            IStaticDataService staticDataService, SelectionHandler selectionHandler)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _selectionHandler = selectionHandler;
        }
        
        public void CreateUIRoot()
        {
            _uiRoot = _assetProvider.Instantiate<GameObject>(AssetPath.UIRootPath).transform;
        }

        public void CreateHUD()
        {
           GameObject hud = _assetProvider.Instantiate<GameObject>(AssetPath.UIHudPath);
           hud.transform.SetParent(_uiRoot);

           foreach (SelectionButton button in hud.GetComponentsInChildren<SelectionButton>())
               button.Init(_selectionHandler);
        }
    }
}