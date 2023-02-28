using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Data.StaticData.ResourceNode
{
    [CreateAssetMenu(fileName = "Resource", menuName = "Resources/Resource")]
    public class ResourceData : ScriptableObject
    {
        public Resource Prefab;
        public ResourceType Type;
    }
}