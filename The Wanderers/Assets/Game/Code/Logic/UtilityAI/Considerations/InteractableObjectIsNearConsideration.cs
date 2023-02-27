﻿using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "InteractableObjectIsNear", 
        menuName = "UtilityAI/Considerations/InteractableObjectIsNear")]
    public class InteractableObjectIsNearConsideration : Consideration
    {
        public override float GetScore(AIContext context, IContextProvider contextProvider)
        {
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