using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "IsHomeowner", menuName = "UtilityAI/Considerations/IsHomeowner")]
    public class IsHomeownerConsideration : Consideration
    {
        public override float GetScore(AIContext context)
        {
            Score = context.Homeowner ? 1f : 0f;
            return Score;
        }
    }
}