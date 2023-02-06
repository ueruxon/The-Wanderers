using System;
using Game.Code.Core;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.UtilityAI;
using Game.Code.Logic.UtilityAI.Context;
using Game.Code.Services.UnitTask;
using UnityEngine;

namespace Game.Code.Logic.Units
{
    public enum UnitState
    {
        Idle,
        Working
    }
    
    [RequireComponent(typeof(AIBrain), typeof(AIPlanner))]
    public class Unit : MonoBehaviour
    {
        public event Action<Unit, UnitTask> TaskCompleted;

        [SerializeField] private AISensor _aiSensor;
        [SerializeField] private MovementSystemBase _movementSystem;
        [SerializeField] private BehaviorData _behaviorData;
        [SerializeField] private AttachedResource _attachedResource;

        private DynamicGameContext _dynamicGameContext;
        private IUnitTaskService _taskService;

        private AIPlanner _aiPlanner;
        private AIContext _aiContext;
        private AIBrain _aiBrain;

        private Animator _animatorController;
        private Resource _resourceInHand;

        public UnitState _currentState;
        
        public void Init(DynamicGameContext dynamicGameContext, IUnitTaskService taskService)
        {
            _dynamicGameContext = dynamicGameContext;
            _taskService = taskService;
            _animatorController = GetComponent<Animator>();
            _movementSystem.Init(_behaviorData.MovementProps);

            _aiSensor.Init();
            _aiContext = new AIContext(_dynamicGameContext, _movementSystem, _aiSensor, _behaviorData, _animatorController, this);

            _aiPlanner = GetComponent<AIPlanner>();
            _aiPlanner.Init(this, dynamicGameContext, _aiContext, _taskService);
            _aiPlanner.TaskCompleted += OnUnitTaskCompleted;
            _aiPlanner.TaskReceived += OnUnitTaskReceived;

            _aiBrain = GetComponent<AIBrain>();
            _aiBrain.Init(_aiContext, _aiPlanner);
            
            _attachedResource.Init(transform);

            SetState(UnitState.Idle);
        }

        public bool IsAvailable()
        {
            if (_currentState != UnitState.Working)
                return true;

            return false;
        }

        public void AttachResource(Resource resource)
        {
            _attachedResource.Attach(resource);
            
            // _resourceInHand = resource;
            // _resourceInHand.transform.SetParent(_testAttachPoint);
            // _resourceInHand.transform.position = _testAttachPoint.position;
        }

        public void DetachResource()
        {
            _attachedResource.Detach();
            
            // Destroy(_resourceInHand.gameObject);
            // _resourceInHand = null;
        }

        private void Update()
        {
            _aiBrain.Decide();
        }

        private void FixedUpdate() => 
            _aiSensor.FindObjects();

        private void SetState(UnitState nextState) => 
            _currentState = nextState;

        private void OnUnitTaskReceived(AIContext context) => 
            SetState(UnitState.Working);

        private void OnUnitTaskCompleted(UnitTask task)
        {
            SetState(UnitState.Idle);
            TaskCompleted?.Invoke(this, task);
        }

        private void OnDestroy()
        {
            _aiPlanner.Cleanup();
            _aiPlanner.TaskCompleted -= OnUnitTaskCompleted;
            _aiPlanner.TaskReceived -= OnUnitTaskReceived;
        }
    }
    
    [Serializable]
    public class BehaviorData
    {
        public MovementProperties MovementProps;
        public int InventoryCapacity;
    }

    [Serializable]
    public class MovementProperties
    {
        public float Speed;
        public float AngularSpeed;
        public float Acceleration;
        public float StoppingDistance;
    }
}