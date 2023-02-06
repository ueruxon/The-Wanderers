using Game.Code.Commander;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "PlacementIsNear", 
        menuName = "UtilityAI/Considerations/PlacementIsNear")]
    public class PlacementIsNearConsideration : Consideration
    {
        public override float GetScore(AIContext context)
        {
            if (context.ActionCommand is GrabResourceCommand)
            {
                Score = context.Sensor.IsPlacementObject() ? 1 : 0;
                return Score;
            }
            
            Score = 0;
            return Score;
        }
    }
}