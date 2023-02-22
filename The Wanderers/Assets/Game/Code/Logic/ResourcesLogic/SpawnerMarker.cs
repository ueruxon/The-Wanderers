using UnityEngine;

namespace Game.Code.Logic.ResourcesLogic
{
    public class SpawnerMarker : MonoBehaviour
    {
        public ResourceType ResourceType;
        
        private void OnDrawGizmos()
        {
            if (ResourceType == ResourceType.Wood)
                Gizmos.color = Color.yellow;
            if (ResourceType == ResourceType.Stone)
                Gizmos.color = Color.gray;
            if (ResourceType == ResourceType.Coal)
                Gizmos.color = Color.cyan;

            Gizmos.DrawSphere(transform.position, .5f);
        }
    }
}