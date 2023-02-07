using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "ReachedDestination", menuName = "UtilityAI/Considerations/ReachedDestination")]
    public class ReachedDestinationConsideration : Consideration
    {
        public override float GetScore(AIContext context)
        {
            Score = context.MovementSystem.ReachedDestination() ? 1f : 0f;
            return Score;
        }
    }
}