using Game.Code.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Code.Logic.ResourcesLogic
{
    public class WorkIndicator : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private CanvasGroup _canvas;

        public void Init()
        {
            // передавать и устанавливать иконку

            Hide();
        }

        public void Show() => 
            _canvas.SetActive(true);

        public void Hide() => 
            _canvas.SetActive(false);
    }
}