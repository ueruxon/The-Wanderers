using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class IdleAction : UtilityAction
    {
        protected override void OnTaskReceived(AIContext context)
        {
            ICommand command = context.ActionCommand;

            if (command is MiningCommand treeCommand)
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
        
        public override void Execute(AIContext context, IContextProvider contextProvider)
        {
            
        }
    }
}