using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "RemainingDistanceTo", menuName = "UtilityAI/Considerations/RemainingDistanceTo")]
    public class RemainingDistanceToConsideration : Consideration
    {
        public override float GetScore(AIContext context)
        {
            Score = context.MovementSystem.ReachedDestination() ? 0f : 1;
            return Score;
        }
    }
}