using System.Collections.Generic;
using Game.Code.Infrastructure.Context;
using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.Buildings.ProductionBuildings;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "HasJob", menuName = "UtilityAI/Considerations/HasJob")]
    public class HasJobConsideration : Consideration
    {
        public override float GetScore(IContextProvider contextProvider)
        {
            DynamicGameContext context = contextProvider.GetContext().GetGlobalDynamicContext();
            Villager villager = contextProvider.GetContext<VillagerContext>().CurrentActor;
            List<IProductionBuilding> productionBuildings = context.GetProductionBuildings();

            foreach (IProductionBuilding building in productionBuildings)
            {
                if (building.IsAvailable() || building.IsReserved(villager))
                {
                    Score = 1;
                    return Score;
                }
            }
            
            Score = 0;
            return Score;
        }
    }
}