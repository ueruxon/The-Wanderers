using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI
{
    public class AISensor : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactableObjectMask;
        [SerializeField] private float _searchRadius = 3f;
        
        private AIContext _aiContext;

        private int _interactableObjectLayer;
        private int _placementObjectLayer;
        private int _resourceNodeLayer;
        private int _resourceLayer;

        private readonly Collider[] _colliders = new Collider[3];

        private int _nearbyObjectCount;

        public void Init()
        {
            _nearbyObjectCount = 0;
            
            _interactableObjectLayer = LayerMask.NameToLayer("InteractableObject");
            _placementObjectLayer = LayerMask.NameToLayer("PlacementObject");
            _resourceNodeLayer = LayerMask.NameToLayer("ResourceNode");
            _resourceLayer = LayerMask.NameToLayer("Resource");
        }

        public void FindObjects() => 
            _nearbyObjectCount = Physics.OverlapSphereNonAlloc(transform.position, _searchRadius, _colliders, _interactableObjectMask);

        public bool IsInteractionObject()
        {
            for (int i = 0; i < _nearbyObjectCount; i++)
            {
                if (_interactableObjectLayer == _colliders[i].gameObject.layer)
                    return true;
            }
            
            return false;
        }
        
        public bool IsResourceNode()
        {
            for (int i = 0; i < _nearbyObjectCount; i++)
            {
                if (_resourceNodeLayer == _colliders[i].gameObject.layer)
                    return true;
            }
            
            return false;
        }
        
        public bool IsResourceObject()
        {
            for (int i = 0; i < _nearbyObjectCount; i++)
            {
                if (_resourceLayer == _colliders[i].gameObject.layer)
                    return true;
            }
            
            return false;
        }
        
        public bool IsPlacementObject()
        {
            for (int i = 0; i < _nearbyObjectCount; i++)
            {
                if (_placementObjectLayer == _colliders[i].gameObject.layer)
                    return true;
            }
            
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _searchRadius);
        }
    }
}