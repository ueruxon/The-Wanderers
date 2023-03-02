using UnityEngine;

namespace Game.Code.Logic.Buildings
{
    public abstract class Building : MonoBehaviour, IBuilding
    {
        [SerializeField] private Transform _enterPoint;
        
        public Transform GetEnterPoint() => 
            _enterPoint;
    }
}