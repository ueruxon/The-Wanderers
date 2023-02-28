using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class PickupItemAction : UtilityAction
    {
        private Transform _target;
        
        public override void OnEnter(IContextProvider contextProvider)
        {
            base.OnEnter(contextProvider);
            
            ICommand command = contextProvider.GetContext().ActionCommand;
            
            if (command is GrabResourceCommand grabCommand)
            {
                Resource resource = grabCommand.Resource;
                
                resource.Pickup();
                
                contextProvider.GetContext<VillagerContext>().CurrentActor.AttachResource(resource);
                contextProvider.GetContext<VillagerContext>().SetPickupResource(resource);
                
                //context.MoveTarget = grabCommand.TargetTo;

                _target = grabCommand.Goal;
            }
        }

        public override void Execute(IContextProvider contextProvider)
        {
            // может какая-то анимация поднятия, или еще че
            // после того как подняли
            
            contextProvider.GetContext().MoveTarget = _target;
            CurrentActionStatus = ActionStatus.Completed;
        }
    }
}