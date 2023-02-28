using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "ItemInHand", menuName = "UtilityAI/Considerations/ItemInHand")]
    public class ItemInHandConsideration : Consideration
    {
        public override float GetScore(IContextProvider contextProvider)
        {
            Score = contextProvider.GetContext<VillagerContext>().PickupResource is not null ? 1 : 0;
            return Score;
        }
    }
}