using UnityEngine;

namespace Game.Code.Logic.ResourcesLogic
{
    public class Resource : MonoBehaviour
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private Collider _collider;
        
        private bool _isPickup = false;

        public bool IsAvailable() => 
            _isPickup == false;

        public void Drop()
        {
            _collider.enabled = true;
            _isPickup = false;
        }

        public void Pickup()
        {
            _isPickup = true;
            _collider.enabled = false;
        }
    } 
}