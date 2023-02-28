using UnityEngine;

namespace Game.Code.Data
{
    [CreateAssetMenu(fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public int InitialVillagerCount = 3;
    }
}