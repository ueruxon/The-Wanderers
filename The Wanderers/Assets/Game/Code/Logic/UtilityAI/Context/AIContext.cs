using Game.Code.Logic.Actors;
using Game.Code.Logic.Game;
using Game.Code.Logic.UtilityAI.Commander;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Context
{
    public abstract class AIContext
    {
        private readonly DynamicGameContext _dynamicGameContext;
        private readonly AISensor _aiSensor;
        private readonly BehaviorData _behaviorData;
        private readonly Animator _animatorController;
        
        public AISensor Sensor => _aiSensor;

        private Transform _target;
        public Transform MoveTarget
        {
            get => _target;
            set
            {
                _target = value;
                // пока как затычка
                MovementSystem.CalculatePath(_target.position);
            }
        }

        public MovementSystemBase MovementSystem { get; }
        public Transform ActorTransform { get; }
        public ICommand ActionCommand { get; private set;}
        public bool IsGlobalCommand { get; private set; }
        
        public AIContext(DynamicGameContext dynamicGameContext, MovementSystemBase movementSystemBase,
            AISensor aiSensor, BehaviorData behaviorData, Animator animatorController, Actor actor)
        {
            _dynamicGameContext = dynamicGameContext;
            _aiSensor = aiSensor;
            _behaviorData = behaviorData;
            _animatorController = animatorController;
            
            MovementSystem = movementSystemBase;
            ActorTransform = actor.transform;
            
            //MoveTarget = CurrentActor.transform;
        }

        public DynamicGameContext GetGlobalDynamicContext() => 
            _dynamicGameContext;
        
        public BehaviorData GetBehaviorData() => 
            _behaviorData;

        public Animator GetAnimatorController() => 
            _animatorController;
        
        
        public void SetActionCommand(ICommand command)
        {
            ActionCommand = command;
            IsGlobalCommand = command is not IdleCommand;
        }
    }
}