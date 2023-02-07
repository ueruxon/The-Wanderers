using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "NotReachedDestination", menuName = "UtilityAI/Considerations/NotReachedDestination")]
    public class NotReachedDestinationConsideration : Consideration
    {
        public override float GetScore(AIContext context)
        {
            Score = context.MovementSystem.ReachedDestination() ? 0f : 1f;
            return Score;;
        }
    }
}