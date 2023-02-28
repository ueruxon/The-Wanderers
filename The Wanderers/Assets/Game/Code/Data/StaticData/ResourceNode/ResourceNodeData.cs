using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Data.StaticData.ResourceNode
{
    [CreateAssetMenu(fileName = "ResourceNode", menuName = "Resources/New ResourceNode")]
    public class ResourceNodeData : ScriptableObject
    {
        public Logic.ResourcesLogic.ResourceNode Prefab;
        public ResourceType Type;
        
        [Range(1, 10)] public int HitToDestroy = 3;
        [Range(10, 60)] private int MaxTimeToRespawn = 40;
        [Range(10, 60)] private int MinToRespawn = 20;
        public int TimeToRespawn => Random.Range(MinToRespawn, MaxTimeToRespawn);
    }
}