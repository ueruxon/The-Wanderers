using System.Collections;
using System.Collections.Generic;
using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.Buildings.ProductionBuildings;
using Game.Code.Logic.UtilityAI.Context;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class JobAction : UtilityAction
    {
        private IProductionBuilding _productionBuilding;

        public override void OnEnter(IContextProvider contextProvider)
        {
            base.OnEnter(contextProvider);
            
            List<IProductionBuilding> buildings = contextProvider
                .GetContext()
                .GetGlobalDynamicContext()
                .GetProductionBuildings();
            
            foreach (IProductionBuilding prodBuilding in buildings)
            {
                if (prodBuilding.IsAvailable())
                {
                    _productionBuilding = prodBuilding;
                    _productionBuilding.PrepareForWork(contextProvider.GetContext<VillagerContext>().CurrentActor);

                    contextProvider
                        .GetContext()
                        .MovementSystem
                        .SetDestination(_productionBuilding.GetEnterPoint().position);
                    
                    contextProvider
                        .GetContext()
                        .GetAnimatorController()
                        .SetBool("IsWalking", true);

                    break;
                }
            }
        }

        public override void Execute(IContextProvider contextProvider)
        {
            if (_productionBuilding is null)
                return;
            
            if (contextProvider.GetContext().MovementSystem.ReachedDestination())
            {
                //contextProvider.GetContext().MovementSystem.Stop();
                contextProvider
                    .GetContext()
                    .GetAnimatorController()
                    .SetBool("IsWalking", false);

                _productionBuilding.Interaction();
            }
        }

        public override void OnExit(IContextProvider contextProvider)
        {
            base.OnExit(contextProvider);

            // only for test
            if (_productionBuilding is not null)
            {
                _productionBuilding.ClearWorkspace();
                _productionBuilding = null;
            }

            contextProvider
                .GetContext()
                .GetAnimatorController()
                .SetBool("IsWalking", false);
        }
    }
}