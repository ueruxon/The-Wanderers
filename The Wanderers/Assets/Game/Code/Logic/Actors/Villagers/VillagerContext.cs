using Game.Code.Logic.Buildings;
using Game.Code.Logic.Game;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.UtilityAI;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.Actors.Villagers
{
    public class VillagerContext : AIContext
    {
        public Villager CurrentActor { get; }
        
        public Resource PickupResource { get; private set;}
        
        private House _houseProperty;
        public bool Homeowner;
        
        public VillagerContext(DynamicGameContext dynamicGameContext, 
            MovementSystemBase movementSystemBase, 
            AISensor aiSensor, BehaviorData behaviorData, 
            Animator animatorController, 
            Actor actor)
            :
            base(dynamicGameContext, 
                movementSystemBase,
                aiSensor, 
                behaviorData, 
                animatorController, 
                actor)
        {
            CurrentActor = actor as Villager;
        }

        public void SetPickupResource(Resource availableResource) => 
            PickupResource = availableResource;
        
        public void RegisterHome(House house)
        {
            Homeowner = true;
            _houseProperty = house;
        }

        public House GetHouse() => 
            _houseProperty;
    }
}