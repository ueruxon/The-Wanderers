using System.Collections.Generic;
using Game.Code.Core;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.Units;
using Game.Code.Logic.UtilityAI.Commander;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Context
{
    // контекст с данными конкретно для этого юнита
    public class AIContext
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
        public Actor CurrentActor { get; }
        public Resource PickupResource { get; private set;}
        public ICommand ActionCommand { get; private set;}
        public bool IsGlobalCommand { get; private set; }

        private House _houseProperty;
        public bool Homeowner;

        public AIContext(DynamicGameContext dynamicGameContext, MovementSystemBase movementSystemBase,
            AISensor aiSensor, BehaviorData behaviorData, Animator animatorController, Actor actor)
        {
            _dynamicGameContext = dynamicGameContext;
            _aiSensor = aiSensor;
            _behaviorData = behaviorData;
            _animatorController = animatorController;
            
            MovementSystem = movementSystemBase;
            CurrentActor = actor;
            
            MoveTarget = CurrentActor.transform;
        }

        public DynamicGameContext GetGlobalDynamicContext() => 
            _dynamicGameContext;

        // нужна какая-то дата конкретно для этого монобеха
        public BehaviorData GetBehaviorData() => 
            _behaviorData;

        public Animator GetAnimatorController() => 
            _animatorController;

        public void SetPickupResource(Resource availableResource) => 
            PickupResource = availableResource;

        public void SetActionCommand(ICommand command)
        {
            ActionCommand = command;
            IsGlobalCommand = command is not IdleCommand;
        }

        public void RegisterHome(House house)
        {
            Homeowner = true;
            _houseProperty = house;
        }

        public House GetHouse() => 
            _houseProperty;
    }
}