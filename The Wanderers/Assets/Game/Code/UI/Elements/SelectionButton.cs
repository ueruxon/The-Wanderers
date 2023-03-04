using System;
using Game.Code.Logic.Selection;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Code.UI.Elements
{
    public class SelectionButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private SelectionMode _selectionMode;
        [Space(4)]
        [SerializeField] private SelectionButtonSettings _buttonSettings;

        private SelectionHandler _selectionHandler;

        public void Init(SelectionHandler selectionHandler)
        {
            _selectionHandler = selectionHandler;
            _selectionHandler.SelectionModeChanged += OnSelectionModeChanged;
            _button.onClick.AddListener(OnSelected);

            _buttonSettings.SelectImage.color = _buttonSettings.DefaultColor;
        }

        private void OnSelected() => 
            _selectionHandler.SetSelectMode(_selectionMode);

        private void OnSelectionModeChanged(SelectionMode selectionMode)
        {
            _buttonSettings.SelectImage.color = 
                selectionMode == _selectionMode 
                    ? _buttonSettings.SelectColor 
                    : _buttonSettings.DefaultColor;
        }

        private void OnDestroy() => 
            _selectionHandler.SelectionModeChanged -= OnSelectionModeChanged;
    }

    [Serializable]
    public class SelectionButtonSettings
    {
        public Image SelectImage;
        public Color DefaultColor;
        public Color SelectColor;
    }
}