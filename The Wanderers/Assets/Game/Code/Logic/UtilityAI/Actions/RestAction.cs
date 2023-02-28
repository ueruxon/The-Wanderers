using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.UtilityAI.Context;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class RestAction : UtilityAction
    {
        private bool _alreadyRest;

        public override void OnEnter(IContextProvider contextProvider)
        {
            base.OnEnter(contextProvider);

            _alreadyRest = false;

            House house = contextProvider.GetContext<VillagerContext>().GetHouse();

            contextProvider
                .GetContext()
                .MovementSystem
                .SetDestination(house.GetEnterPoint().position);
            contextProvider
                .GetContext()
                .GetAnimatorController()
                .SetBool("IsWalking", true);
        }

        public override void Execute(IContextProvider contextProvider)
        {
            if (contextProvider.GetContext().MovementSystem.ReachedDestination())
            {
                if (_alreadyRest == false)
                {
                    _alreadyRest = true;

                    VillagerContext villagerContext = contextProvider.GetContext<VillagerContext>();
                    Villager villager = villagerContext.CurrentActor;
                    
                    villagerContext.MovementSystem.Stop();
                    villager.Hide();
                    villager.transform.position = villagerContext
                        .GetHouse()
                        .GetEnterPoint()
                        .position;
                }
            }
        }

        public override void OnExit(IContextProvider contextProvider)
        {
            base.OnExit(contextProvider);
            
            contextProvider.GetContext<VillagerContext>().CurrentActor.Show();
            contextProvider
                .GetContext()
                .GetAnimatorController()
                .SetBool("IsWalking", false);
        }
    }
}