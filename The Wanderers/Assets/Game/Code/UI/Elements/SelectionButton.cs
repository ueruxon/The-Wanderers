using Game.Code.Logic.Selection;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Code.UI.Elements
{
    public class SelectionButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private SelectionMode _selectionMode;

        private SelectionHandler _selectionHandler;

        public void Init(SelectionHandler selectionHandler)
        {
            _selectionHandler = selectionHandler;
            _button.onClick.AddListener(OnSelected);
        }

        private void OnSelected() => 
            _selectionHandler.SetSelectMode(_selectionMode);
    }
}