using System;
using Game.Code.Logic.Units;

namespace Game.Code.Services.UnitTask
{
    public interface IUnitTaskService : IService
    {
        public event Action NotifyUnit;
        public bool HasTask();
        public UnitTask GetTask(Unit unit);
        public void AddTask(UnitTask task);
    }
}