using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class MoveToAction : UtilityAction
    {
        public override void Execute(AIContext context)
        {
            Vector3 targetPosition = context.MoveTarget.position;
            
            MoveTo(context, targetPosition);
        }
        
         protected override void MoveTo(AIContext context, Vector3 target)
         {
             context.MovementSystem.SetDestination(target);

             if (context.MovementSystem.ReachedDestination())
             {
                 context.MovementSystem.Stop();
                 context.GetAnimatorController().SetBool("IsWalking", false);
                 CurrentActionStatus = ActionStatus.Completed;
             }
             else
             {
                 context.GetAnimatorController().SetBool("IsWalking", true);
                 CurrentActionStatus = ActionStatus.Running;
             }
         }
    }
}