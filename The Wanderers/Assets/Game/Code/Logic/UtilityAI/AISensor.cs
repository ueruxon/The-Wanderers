using System;
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
        private int _resourceLayer;

        private readonly Collider[] _colliders = new Collider[3];

        public void Init(AIContext aiContext)
        {
            _aiContext = aiContext;
            
            _interactableObjectLayer = LayerMask.NameToLayer("InteractableObject");
            _placementObjectLayer = LayerMask.NameToLayer("PlacementObject");
            _resourceLayer = LayerMask.NameToLayer("Resource");
        }

        // private void FixedUpdate()
        // {
        //     int count = Physics.OverlapSphereNonAlloc(
        //         transform.position, _checkInteractableObjectRadius,
        //         _interactableColliders, _interactableObjectMask, QueryTriggerInteraction.Ignore);
        //
        //     if (count > 0)
        //     {
        //         for (int i = 0; i < _interactableColliders.Length; i++)
        //         {
        //             if (ReferenceEquals(_interactableColliders[i], null) == false)
        //             {
        //                 int hitLayer = _interactableColliders[i].gameObject.layer;
        //                 
        //                 print(_interactableColliders[i].gameObject);
        //                 
        //                 if (hitLayer == _interactableObjectLayer)
        //                     _aiContext.IsInteractionObject = true;
        //
        //                 if (hitLayer == _resourceLayer)
        //                     _aiContext.IsResourceObject = true;
        //
        //                 if (hitLayer == _placementObjectLayer)
        //                     _aiContext.IsPlacementObject = true;
        //             }
        //         }
        //     }
        //     else
        //     {
        //         DisableAll();
        //     }
        // }

        public void Find()
        {
            SetFalseAll();
            
            int count = Physics.OverlapSphereNonAlloc(transform.position, _searchRadius, _colliders, _interactableObjectMask);

            if (count == 0)
                return;

            for (int i = 0; i < _colliders.Length; i++)
            {
                if (ReferenceEquals(_colliders[i], null) == false)
                {
                    int hitLayer = _colliders[i].gameObject.layer;
                    
                    if (hitLayer == _interactableObjectLayer)
                        _aiContext.IsInteractionObject = true;
                    
                    if (hitLayer == _resourceLayer)
                        _aiContext.IsResourceObject = true;
                    
                    if (hitLayer == _placementObjectLayer)
                        _aiContext.IsPlacementObject = true;
                }
            }

            for (int i = 0; i < _colliders.Length; i++) 
                _colliders[i] = null;
        }

        private void SetFalseAll()
        {
            _aiContext.IsInteractionObject = false;
            _aiContext.IsResourceObject = false;
            _aiContext.IsPlacementObject = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _searchRadius);
        }
    }
}