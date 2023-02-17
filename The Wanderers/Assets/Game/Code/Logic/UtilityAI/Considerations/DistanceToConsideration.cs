using Game.Code.Logic.Units;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "DistanceTo", menuName = "UtilityAI/Considerations/DistanceTo")]
    public class DistanceToConsideration : Consideration
    {
        public override float GetScore(AIContext context)
        {
            BehaviorData behaviorData = context.GetBehaviorData();
            float distanceToTarget = Vector3.Distance(context.MoveTarget.position, context.CurrentActor.transform.position);

            Score = distanceToTarget < behaviorData.MovementProps.StoppingDistance + 1f ? 0.01f : 1f;
            return Score;
        }
    }
}