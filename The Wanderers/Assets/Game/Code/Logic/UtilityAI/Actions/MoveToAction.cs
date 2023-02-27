using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class MoveToAction : UtilityAction
    {
        public override void OnEnter(AIContext context, IContextProvider contextProvider)
        {
            base.OnEnter(context, contextProvider);
            
            context.GetAnimatorController().SetBool("IsWalking", true);
        }
        
        public override void Execute(AIContext context, IContextProvider contextProvider)
        {
            Vector3 targetPosition = context.MoveTarget.position;
            
            MoveTo(context, targetPosition);
        }
        
        protected override void MoveTo(AIContext context, Vector3 target) => 
            context.MovementSystem.SetDestination(target);

        public override void OnExit(AIContext context, IContextProvider contextProvider)
        {
            base.OnExit(context, contextProvider);

            context.MovementSystem.Stop();
            context.GetAnimatorController().SetBool("IsWalking", false);
        }
    }
}