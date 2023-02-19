using System;
using Game.Code.Core;
using Game.Code.Infrastructure.Services.UnitTask;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.UtilityAI;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.Units
{
    public enum ActorState
    {
        Idle,
        Working
    }
    
    [RequireComponent(typeof(AIBrain), typeof(AIPlanner))]
    public class Actor : MonoBehaviour
    {
        public event Action<Actor, GlobalActorTask> TaskCompleted;

        [SerializeField] private AISensor _aiSensor;
        [SerializeField] private MovementSystemBase _movementSystem;
        [SerializeField] private BehaviorData _behaviorData;
        [SerializeField] private AttachedResource _attachedResource;
        
        private DynamicGameContext _dynamicGameContext;
        private IActorTaskService _taskService;

        private AIPlanner _aiPlanner;
        private AIContext _aiContext;
        private AIBrain _aiBrain;

        private Animator _animatorController;
        private Resource _resourceInHand;

        private ActorState _currentState;
        
        public void Init(DynamicGameContext dynamicGameContext, IActorTaskService taskService)
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

            SetState(ActorState.Idle);
        }

        public bool IsAvailable() => 
            _currentState != ActorState.Working;

        public void AttachResource(Resource resource) => 
            _attachedResource.Attach(resource);

        public void DetachResource() => 
            _attachedResource.Detach();

        public void RegisterHome(House house) => 
            _aiContext.RegisterHome(house);
        
        public void Hide()
        {
            _animatorController.enabled = false;
            _behaviorData.Visual.SetActive(false);
            //_movementSystem.SetActive(false);
        }

        public void Show()
        {
            _animatorController.enabled = true;
            _behaviorData.Visual.SetActive(true);
            //_movementSystem.SetActive(true);
        }

        private void Update()
        {
            _aiBrain.Decide();
        }

        private void FixedUpdate()
        {
            _aiSensor.FindObjects();
        }

        private void SetState(ActorState nextState) => 
            _currentState = nextState;

        private void OnUnitTaskReceived(AIContext context) => 
            SetState(ActorState.Working);

        private void OnUnitTaskCompleted(GlobalActorTask task)
        {
            SetState(ActorState.Idle);
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
        public GameObject Visual;
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