using System.Collections.Generic;
using Game.Code.Logic.Units;
using UnityEngine;

namespace Game.Code.Logic.Buildings
{
    public class House : Building
    {
        [SerializeField] private int _houseCapacity = 2;
        [SerializeField] private Transform _interactionPoint;

        private List<Unit> _homeowners;
        
        public void Init()
        {
            _homeowners = new List<Unit>(_houseCapacity);
        }

        public bool CanRegisterUnit() => 
            _homeowners.Count < _houseCapacity;

        public void RegisterUnit(Unit unit) => 
            _homeowners.Add(unit);

        public void RemoveUnit(Unit unit) => 
            _homeowners.Remove(unit);

        public Transform GetEnterPoint() => 
            _interactionPoint;
    }
}