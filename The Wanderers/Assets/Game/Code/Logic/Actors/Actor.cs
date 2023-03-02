using System;
using Game.Code.Data.StaticData.Actors;
using Game.Code.Infrastructure.Context;
using Game.Code.Infrastructure.Services.ActorTask;
using Game.Code.Logic.UtilityAI;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.Actors
{
    public enum ActorState
    {
        Idle,
        Working
    }
    
    [RequireComponent(typeof(AIBrain), typeof(AIPlanner))]
    public abstract class Actor : MonoBehaviour
    {
        public event Action<Actor, GlobalActorTask> TaskCompleted;

        [SerializeField] protected AISensor _aiSensor;
        [SerializeField] protected MovementSystemBase _movementSystem;
        [SerializeField] protected BehaviorData _behaviorData;

        protected IActorTaskService TaskService;
        protected AIPlanner AiPlanner;
        protected AIBrain AiBrain;
        
        private ActorData _actorData;
        protected Animator AnimatorController;

        private ActorState _currentState;

        public void Construct(ActorData actorData,
            DynamicGameContext gameContext,
            IActorTaskService taskService)
        {
            _actorData = actorData;
            TaskService = taskService;
            AnimatorController = GetComponent<Animator>();
            AiPlanner = GetComponent<AIPlanner>();
            AiBrain = GetComponent<AIBrain>();

            CreateContext(gameContext);
            Setup();
        }

        protected abstract void CreateContext(DynamicGameContext dynamicGameContext);

        protected abstract void Setup();

        protected void Initialize(IContextProvider contextProvider)
        {
            _movementSystem.Init(_behaviorData.MovementProps);
            _aiSensor.Init();
            AiBrain.Init(AiPlanner, contextProvider);
            AiPlanner.Init(this, contextProvider, TaskService);
            AiPlanner.TaskCompleted += OnUnitTaskCompleted;
            AiPlanner.TaskReceived += OnUnitTaskReceived;
        }

        public bool IsAvailable() => 
            _currentState != ActorState.Working;
        
        private void Update()
        {
            AiBrain.Decide();
        }

        private void FixedUpdate()
        {
            _aiSensor.FindObjects();
        }

        protected void SetState(ActorState nextState) => 
            _currentState = nextState;

        private void OnUnitTaskReceived(IContextProvider contextProvider) => 
            SetState(ActorState.Working);

        private void OnUnitTaskCompleted(GlobalActorTask task)
        {
            SetState(ActorState.Idle);
            TaskCompleted?.Invoke(this, task);
        }

        private void OnDestroy()
        {
            AiPlanner.Cleanup();
            AiPlanner.TaskCompleted -= OnUnitTaskCompleted;
            AiPlanner.TaskReceived -= OnUnitTaskReceived;
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