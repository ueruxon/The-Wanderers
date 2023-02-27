using Game.Code.Logic.Buildings;
using Game.Code.Logic.Game;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.Actors.Villagers
{
    public class Villager : Actor
    {
        [SerializeField] private AttachedResource _attachedResource;

        private VillagerContext _context;
        private AIContextProvider<VillagerContext> _contextProvider;
        private Resource _resourceInHand;
        
        protected override void CreateContext(DynamicGameContext dynamicGameContext)
        {
            _context = new VillagerContext(dynamicGameContext,
                _movementSystem, _aiSensor, _behaviorData, AnimatorController, this);
            _contextProvider = new AIContextProvider<VillagerContext>(_context);
        }

        protected override void Setup()
        {
            Initialize(_context, _contextProvider);
            
            _attachedResource.Init(transform);
            SetState(ActorState.Idle);
        }

        public void Hide()
        {
            AnimatorController.enabled = false;
            _behaviorData.Visual.SetActive(false);
            //_movementSystem.SetActive(false);
        }

        public void Show()
        {
            AnimatorController.enabled = true;
            _behaviorData.Visual.SetActive(true);
            //_movementSystem.SetActive(true);
        }
        
        public void AttachResource(Resource resource) => 
            _attachedResource.Attach(resource);

        public void DetachResource() => 
            _attachedResource.Detach();
        
        public void RegisterHome(House house) => 
            _context.RegisterHome(house);
    }
}