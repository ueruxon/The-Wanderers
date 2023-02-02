using Game.Code.Commander;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class IdleAction : UtilityAction
    {
        protected override void OnTaskReceived(AIContext context)
        {
            ICommand command = context.ActionCommand;

            if (command is ChopTreeCommand treeCommand)
            {
                ResourceNode resourceNode = treeCommand.ResourceNode;
                
                if (resourceNode.IsAvailableForWork())
                {
                    resourceNode.InWork();
                    
                    context.MoveTarget = resourceNode.transform;
                    CurrentActionStatus = ActionStatus.Completed;
                }
            }

            if (command is GrabResourceCommand grabCommand)
            {
                Resource resource = grabCommand.Resource;
                if (resource.IsAvailable())
                {
                    context.MoveTarget = resource.transform;
                    CurrentActionStatus = ActionStatus.Completed;
                }
            }
        }


        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);
            
            //context.MovementSystem.Stop();
            //context.GetAnimatorController().SetBool("IsWalking", false);
        }

        public override void Execute(AIContext context)
        {
            // попытаться взять уже добытый ресурс на карте приоритетнее, чем искать ресурную ноду
            // if (context.GetGlobalContext().TryTakeAvailableResource(context.CurrentUnit, out Resource availableResource))
            // {
            //     context.MoveTarget = availableResource.transform;
            //     CurrentActionStatus = ActionStatus.Completed;
            //
            //     // переписать
            //     context.SetPickupResource(availableResource);
            // }
            
            //
            // if (context.TryGetClosestResourceSpawner(out ResourceNodeSpawner nodeSpawner))
            // {
            //     nodeSpawner.InWork();
            //     context.MoveTarget = nodeSpawner.transform;
            //
            //     CurrentActionStatus = ActionStatus.Completed;
            // }
        }
    }
}