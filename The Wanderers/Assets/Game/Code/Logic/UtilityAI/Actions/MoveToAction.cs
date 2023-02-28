using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class MoveToAction : UtilityAction
    {
        public override void OnEnter(IContextProvider contextProvider)
        {
            base.OnEnter(contextProvider);
            
            contextProvider
                .GetContext()
                .GetAnimatorController()
                .SetBool("IsWalking", true);
        }
        
        public override void Execute(IContextProvider contextProvider)
        {
            AIContext context = contextProvider.GetContext();
            
            Vector3 targetPosition = context.MoveTarget.position;
            
            MoveTo(context, targetPosition);
        }
        
        protected override void MoveTo(AIContext context, Vector3 target) => 
            context.MovementSystem.SetDestination(target);

        public override void OnExit(IContextProvider contextProvider)
        {
            base.OnExit(contextProvider);
            
            contextProvider.GetContext().MovementSystem.Stop();
            contextProvider
                .GetContext()
                .GetAnimatorController()
                .SetBool("IsWalking", false);
        }
    }
}