using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "ItemInHand", menuName = "UtilityAI/Considerations/ItemInHand")]
    public class ItemInHandConsideration : Consideration
    {
        public override float GetScore(AIContext context)
        {
            Score = context.PickupResource is not null ? 1 : 0;
            return Score;
        }
    }
}