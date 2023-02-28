using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "ResourceIsNear", 
        menuName = "UtilityAI/Considerations/ResourceIsNear")]
    public class ResourceIsNearConsideration : Consideration
    {
        public override float GetScore(AIContext context, IContextProvider contextProvider)
        {
            if (context.IsGlobalCommand)
            {
                if (context.ActionCommand is GrabResourceCommand)
                {
                    if (context.Sensor.IsResourceObject() 
                        && contextProvider.GetContext<VillagerContext>().PickupResource is null)
                    {
                        Score = 1;
                        return Score;
                    }
                }
            }

            Score = 0;
            return Score;
        }
    }
}