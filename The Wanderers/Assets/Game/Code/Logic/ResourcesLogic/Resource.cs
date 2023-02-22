using UnityEngine;

namespace Game.Code.Logic.ResourcesLogic
{
    public class Resource : MonoBehaviour
    {
        [SerializeField] private Collider _collider;

        private ResourceType _resourceType;
        private bool _isPickup = false;

        public void Init(ResourceType resourceType) => 
            _resourceType = resourceType;

        public bool IsAvailable() => 
            _isPickup == false;

        public ResourceType GetResourceType() => 
            _resourceType;

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