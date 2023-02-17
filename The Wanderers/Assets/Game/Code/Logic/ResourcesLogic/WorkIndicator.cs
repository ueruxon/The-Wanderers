using UnityEngine;
using UnityEngine.UI;

namespace Game.Code.Logic.ResourcesLogic
{
    public class WorkIndicator : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void Init()
        {
            // передавать и устанавливать иконку

            Hide();
        }

        public void Show() => 
            gameObject.SetActive(true);
        
        public void Hide() => 
            gameObject.SetActive(false);
    }
}