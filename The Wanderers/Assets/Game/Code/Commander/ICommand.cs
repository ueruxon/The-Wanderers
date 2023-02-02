using UnityEngine;

namespace Game.Code.Commander
{
    public interface ICommand
    {
        public Transform Target { get; set; }
        public Transform Goal { get; set; }
    }
}