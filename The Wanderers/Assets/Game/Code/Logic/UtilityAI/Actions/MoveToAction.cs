using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class MoveToAction : UtilityAction
    {
        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);
            
            context.GetAnimatorController().SetBool("IsWalking", true);
        }
        
        public override void Execute(AIContext context)
        {
            Vector3 targetPosition = context.MoveTarget.position;
            
            MoveTo(context, targetPosition);
        }
        
        protected override void MoveTo(AIContext context, Vector3 target) => 
            context.MovementSystem.SetDestination(target);

        public override void OnExit(AIContext context)
        {
            base.OnExit(context);
             
            context.MovementSystem.Stop();
            context.GetAnimatorController().SetBool("IsWalking", false);
        }
    }
}