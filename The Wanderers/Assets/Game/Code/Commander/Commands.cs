using System;
using Game.Code.Logic.Buildings;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Commander
{
    public struct IdleCommand : ICommand
    { 
        public Transform Target { get; set; }
        public Transform Goal { get; set; }
    }
    
    public struct ChopTreeCommand : ICommand
    {
        public ResourceNode ResourceNode;
        public Transform Target { get; set; }
        public Transform Goal { get; set; }
    }

    public struct GrabResourceCommand : ICommand
    {
        public Resource Resource;
        public Transform Target { get; set; }
        public Transform Goal { get; set; }
        public Storage Storage;
    }
}