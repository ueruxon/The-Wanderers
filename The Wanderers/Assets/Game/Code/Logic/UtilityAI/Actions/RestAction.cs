using Game.Code.Logic.Buildings;
using Game.Code.Logic.UtilityAI.Context;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class RestAction : UtilityAction
    {
        private bool _alreadyRest;

        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);

            _alreadyRest = false;

            House house = context.GetHouse();

            context.MovementSystem.SetDestination(house.GetEnterPoint().position);
            context.GetAnimatorController().SetBool("IsWalking", true);
        }

        public override void Execute(AIContext context)
        {
            if (context.MovementSystem.ReachedDestination())
            {
                if (_alreadyRest == false)
                {
                    _alreadyRest = true;

                    context.MovementSystem.Stop();
                    context.CurrentActor.Hide();
                    context.CurrentActor.transform.position = context.GetHouse().GetEnterPoint().position;
                }
            }
        }

        public override void OnExit(AIContext context)
        {
            base.OnExit(context);
            
            context.CurrentActor.Show();
            context.GetAnimatorController().SetBool("IsWalking", false);
        }
    }
}