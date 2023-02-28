using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class DropItemAction : UtilityAction
    {
        private Storage _storage;
        
        public override void OnEnter(AIContext context, IContextProvider contextProvider)
        {
            base.OnEnter(context, contextProvider);
            
            ICommand command = context.ActionCommand;
            
            if (command is GrabResourceCommand grabCommand) 
                _storage = grabCommand.Storage;
        }
        
        public override void Execute(AIContext context, IContextProvider contextProvider)
        {
            // анимация бросания вещи?
            // еще чето?

            if (context.MovementSystem.ReachedDestination())
            {
                _storage.AddResource(contextProvider.GetContext<VillagerContext>().PickupResource);
                contextProvider.GetContext<VillagerContext>().SetPickupResource(null);
                contextProvider.GetContext<VillagerContext>().CurrentActor.DetachResource();

                CompleteTask();
                CurrentActionStatus = ActionStatus.Completed;
            }
        }
    }
}