using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class DropItemAction : UtilityAction
    {
        private Storage _storage;
        
        public override void OnEnter(IContextProvider contextProvider)
        {
            base.OnEnter(contextProvider);

            ICommand command = contextProvider.GetContext().ActionCommand;
            
            if (command is GrabResourceCommand grabCommand) 
                _storage = grabCommand.Storage;
        }
        
        public override void Execute(IContextProvider contextProvider)
        {
            // анимация бросания вещи?
            // еще чето?

            if (contextProvider.GetContext().MovementSystem.ReachedDestination())
            {
                VillagerContext villagerContext = contextProvider.GetContext<VillagerContext>();
                
                _storage.AddResource(villagerContext.PickupResource);
                villagerContext.SetPickupResource(null);
                villagerContext.CurrentActor.DetachResource();

                CompleteTask();
                CurrentActionStatus = ActionStatus.Completed;
            }
        }
    }
}