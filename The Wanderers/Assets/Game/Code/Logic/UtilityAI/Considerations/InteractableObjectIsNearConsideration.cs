using Game.Code.Commander;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "InteractableObjectIsNear", 
        menuName = "UtilityAI/Considerations/InteractableObjectIsNear")]
    public class InteractableObjectIsNearConsideration : Consideration
    {
        public override float GetScore(AIContext context)
        {
            if (context.ActionCommand is ChopTreeCommand)
            {
                Score = context.IsInteractionObject ? 1 : 0;
                return Score;
            }
            
            Score = 0;
            return Score;
        }
    }
}