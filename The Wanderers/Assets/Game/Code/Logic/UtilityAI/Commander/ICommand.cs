using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Commander
{
    public interface ICommand
    {
        public Transform Target { get; set; }
        public Transform Goal { get; set; }
    }
}