using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Idle", menuName = "UtilityAI/Considerations/Idle")]
    public class IdleConsideration : Consideration
    {
        public override float GetScore(IContextProvider contextProvider)
        {
            Score = contextProvider.GetContext<VillagerContext>().CurrentActor.IsAvailable() ? 1 : 0;
            //Score = TestValue;
            return Score;
        }
    }
}