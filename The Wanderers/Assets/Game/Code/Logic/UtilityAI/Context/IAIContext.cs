using Game.Code.Logic.Units;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Context
{
    public interface IAIContext
    {
        public BehaviorData GetBehaviorDataData();
        public Animator GetAnimatorController();
    }
}