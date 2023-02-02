using System;
using System.Collections.Generic;
using Game.Code.Common;
using UnityEngine;
using DG.Tweening;
using Game.Code.Extensions;
using MoreMountains.Feedbacks;

namespace Game.Code.Logic.ResourcesLogic
{
    public class ResourceNode : MonoBehaviour, IInteractable
    {
        public event Action NodeDestroyed;

        [SerializeField] private Collider _collider;
        [SerializeField] private List<Transform> _visualVariants;
        [SerializeField] private MMF_Player _demolishFeedback;
        [SerializeField] private MMF_Player _growFeedback;

        private int _hitToSpawnResource = 3;

        private bool _isActive;
        private bool _isAvailableForWork;

        private Transform _visual;

        public void Init()
        {
            _isActive = true;
            _isAvailableForWork = true;
            
            for (int i = 0; i < _visualVariants.Count; i++) 
                _visualVariants[i].gameObject.SetActive(false);

            _visual = _visualVariants.GetRandomItem();
            _visual.gameObject.SetActive(true);
            
            _growFeedback.PlayFeedbacks();
        }

        public bool IsActive() => 
            _isActive;

        public bool IsAvailableForWork() => 
            _isAvailableForWork;

        public void InWork() => 
            _isAvailableForWork = false;

        public void Interact()
        {
            _hitToSpawnResource--;
            
            Shake();

            if (_hitToSpawnResource == 0) 
                Breakdown();
        }
        
        private void Shake()
        {
            _visual.DOShakePosition(0.5f, 0.25f);
            _visual.DOShakeRotation(0.5f, 1f);
            _visual.DOShakeScale(0.5f, 0.2f, 10, 120f);
        }
        
        private void Breakdown()
        {
            _visual.DOKill();
            _isActive = false;
            _collider.enabled = false;
            _demolishFeedback.PlayFeedbacks();
            
            NodeDestroyed?.Invoke();
        }
    }
}