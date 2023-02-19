using Game.Code.Infrastructure.Services.UnitTask;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;
using Game.Code.Logic.UtilityAI.Commander;

namespace Game.Code.Infrastructure.Factories
{
    public class ActorTaskFactory
    {
        public GlobalActorTask CreateGatherResourceTask(Resource resource, Storage storage)
        {
            GrabResourceCommand command = new GrabResourceCommand()
            {
                Resource = resource,
                Target = resource.transform,
                Goal = storage.GetInteractionPoint(),
                Storage = storage
            };

            return new GlobalActorTask(command);
        }

        public GlobalActorTask CreateMiningTask(ResourceNode resourceNode)
        {
            ChopTreeCommand chopTreeCommand = new ChopTreeCommand()
            {
                ResourceNode = resourceNode,
                Target = resourceNode.transform,
                Goal = resourceNode.transform
            };

            return new GlobalActorTask(command: chopTreeCommand, resourceNode.ID);
        }
    }
}