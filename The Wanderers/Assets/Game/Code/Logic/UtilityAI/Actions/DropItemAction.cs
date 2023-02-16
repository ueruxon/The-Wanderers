using Game.Code.Logic.Buildings;
using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class DropItemAction : UtilityAction
    {
        private Storage _storage;
        
        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);
            
            ICommand command = context.ActionCommand;
            
            if (command is GrabResourceCommand grabCommand) 
                _storage = grabCommand.Storage;
        }
        
        public override void Execute(AIContext context)
        {
            // анимация бросания вещи?
            // еще чето?

            if (context.MovementSystem.ReachedDestination())
            {
                // переписать естессна
                _storage.AddResource(context.PickupResource);

                //context.GetGlobalContext().RemoveResource(context.PickupResource);
                context.SetPickupResource(null);
                context.CurrentUnit.DetachResource();

                AIPlanner.CompleteCurrentTask();
                CurrentActionStatus = ActionStatus.Completed;
            }
        }
    }
}