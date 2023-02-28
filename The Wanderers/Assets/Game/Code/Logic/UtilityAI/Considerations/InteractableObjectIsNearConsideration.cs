using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "InteractableObjectIsNear", 
        menuName = "UtilityAI/Considerations/InteractableObjectIsNear")]
    public class InteractableObjectIsNearConsideration : Consideration
    {
        public override float GetScore(IContextProvider contextProvider)
        {
            AIContext context = contextProvider.GetContext();
            
            if (context.IsGlobalCommand)
            {
                if (context.ActionCommand is MiningCommand)
                {
                    Score = context.Sensor.IsResourceNode() ? 1 : 0;
                    return Score;
                }
            }

            Score = 0;
            return Score;
        }
    }
}