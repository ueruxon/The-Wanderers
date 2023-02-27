using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.UtilityAI.Context;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class RestAction : UtilityAction
    {
        private bool _alreadyRest;

        public override void OnEnter(AIContext context, IContextProvider contextProvider)
        {
            base.OnEnter(context, contextProvider);

            _alreadyRest = false;

            House house = contextProvider.GetContext<VillagerContext>().GetHouse();

            context.MovementSystem.SetDestination(house.GetEnterPoint().position);
            context.GetAnimatorController().SetBool("IsWalking", true);
        }

        public override void Execute(AIContext context, IContextProvider contextProvider)
        {
            if (context.MovementSystem.ReachedDestination())
            {
                if (_alreadyRest == false)
                {
                    _alreadyRest = true;

                    context.MovementSystem.Stop();
                    contextProvider.GetContext<VillagerContext>().CurrentActor.Hide();
                    context.CurrentActor.transform.position = contextProvider
                        .GetContext<VillagerContext>()
                        .GetHouse()
                        .GetEnterPoint()
                        .position;
                }
            }
        }

        public override void OnExit(AIContext context, IContextProvider contextProvider)
        {
            base.OnExit(context, contextProvider);
            
            contextProvider.GetContext<VillagerContext>().CurrentActor.Show();
            context.GetAnimatorController().SetBool("IsWalking", false);
        }
    }
}