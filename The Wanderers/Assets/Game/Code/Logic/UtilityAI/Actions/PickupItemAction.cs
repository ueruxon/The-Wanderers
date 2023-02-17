using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class PickupItemAction : UtilityAction
    {
        private Transform _target;
        
        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);
            
            print("pickup");
            
            ICommand command = context.ActionCommand;
            
            if (command is GrabResourceCommand grabCommand)
            {
                Resource resource = grabCommand.Resource;
                
                resource.Pickup();
                context.CurrentActor.AttachResource(resource);
                context.SetPickupResource(resource);
                //context.MoveTarget = grabCommand.TargetTo;

                _target = grabCommand.Goal;
            }
        }

        public override void Execute(AIContext context)
        {
            // может какая-то анимация поднятия, или еще че
            // после того как подняли

            context.MoveTarget = _target;
            CurrentActionStatus = ActionStatus.Completed;
        }
    }
}