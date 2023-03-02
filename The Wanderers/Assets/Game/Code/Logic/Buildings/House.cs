using System.Collections.Generic;
using Game.Code.Logic.Actors;
using UnityEngine;

namespace Game.Code.Logic.Buildings
{
    public class House : Building
    {
        [SerializeField] private int _houseCapacity = 2;

        private List<Actor> _homeowners;
        
        public void Init()
        {
            _homeowners = new List<Actor>(_houseCapacity);
        }

        public bool CanRegisterUnit() => 
            _homeowners.Count < _houseCapacity;

        public void RegisterUnit(Actor actor) => 
            _homeowners.Add(actor);

        public void RemoveUnit(Actor actor) => 
            _homeowners.Remove(actor);
    }
}