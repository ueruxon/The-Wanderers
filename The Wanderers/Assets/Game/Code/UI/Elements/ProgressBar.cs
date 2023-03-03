using Game.Code.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Code.UI.Elements
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Show() => 
            _canvasGroup.SetActive(true);
        
        public void Hide() => 
            _canvasGroup.SetActive(false);

        public void SetValue(float current, float max) => 
            _fillImage.fillAmount = current / max;
    }
}