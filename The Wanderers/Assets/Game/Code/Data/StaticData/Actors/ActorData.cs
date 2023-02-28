using System.Collections.Generic;
using Game.Code.Logic.Actors;
using UnityEngine;

namespace Game.Code.Data.StaticData.Actors
{
    [CreateAssetMenu(fileName = "New Actor", menuName = "Actors/New Actor")]
    public class ActorData : ScriptableObject
    {
        public Actor Prefab;
        public ActorType Type;

        [SerializeField] private List<Color> _bodyColorVariants;
        [SerializeField] private List<Color> _headColorVariants;
    }
}