﻿using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Idle", menuName = "UtilityAI/Considerations/Idle")]
    public class IdleConsideration : Consideration
    {
        public override float GetScore(AIContext context)
        {
            Score = TestValue;
            return Score;
        }
    }
}