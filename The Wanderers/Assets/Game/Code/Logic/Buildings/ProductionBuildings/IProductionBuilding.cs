using System;
using Game.Code.Logic.Actors.Villagers;
using Game.Code.Logic.ResourcesLogic;

namespace Game.Code.Logic.Buildings.ProductionBuildings
{
    public interface IProductionBuilding : IBuilding
    {
        public event Action<Resource> ResourceSpawned; 
        public bool IsAvailable();
        void PrepareForWork(Villager villager);
        public void Interaction();
        bool IsReserved(Villager villager);
        void ClearWorkspace();
    }
}