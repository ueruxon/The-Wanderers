using System;
using System.Collections.Generic;
using Game.Code.Common;
using UnityEngine;
using DG.Tweening;
using Game.Code.Extensions;
using MoreMountains.Feedbacks;

namespace Game.Code.Logic.ResourcesLogic
{
    public enum ResourceNodeState
    {
        Idle,
        InPrepareForWork,
        InWork,
        WorkedOut
    }
    
    public class ResourceNode : MonoBehaviour, IInteractable
    {
        public event Action NodeDestroyed;

        [SerializeField] private Collider _collider;
        [SerializeField] private List<Transform> _visualVariants;
        [SerializeField] private WorkIndicator _workIndicator;
        [SerializeField] private MMF_Player _demolishFeedback;
        [SerializeField] private MMF_Player _growFeedback;

        private Transform _visual;
        
        private ResourceType _currentType;
        private ResourceNodeState _nodeState;

        private int _hitToSpawnResource = 3;
        
        public void Init(ResourceType resourceType)
        {
            _currentType = resourceType;
            _nodeState = ResourceNodeState.Idle;
            
            for (int i = 0; i < _visualVariants.Count; i++) 
                _visualVariants[i].gameObject.SetActive(false);

            _visual = _visualVariants.GetRandomItem();
            _visual.gameObject.SetActive(true);
            
            _workIndicator.Init();
            
            _growFeedback.PlayFeedbacks();
        }

        public bool IsIdle() => 
            _nodeState == ResourceNodeState.Idle;

        public bool IsActive() =>
            _nodeState != ResourceNodeState.WorkedOut;

        public bool IsAvailableForWork() => 
            _nodeState == ResourceNodeState.InPrepareForWork;

        public void Prepare(bool value)
        {
            if (value)
            {
                _nodeState = ResourceNodeState.InPrepareForWork;
                _workIndicator.Show();
            }
            else
            {
                _nodeState = ResourceNodeState.Idle;
                _workIndicator.Hide();
            }
        }

        public void InWork() => 
            _nodeState = ResourceNodeState.InWork;

        public ResourceType GetNodeType() => 
            _currentType;

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
            _collider.enabled = false;
            _demolishFeedback.PlayFeedbacks();

            _nodeState = ResourceNodeState.WorkedOut;
            _workIndicator.Hide();
            
            NodeDestroyed?.Invoke();
        }
    }
}