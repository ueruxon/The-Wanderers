using System;
using UnityEngine;

namespace Game.Code.Common
{
    public static class UniqueIDGenerator
    {
        public static string GenerateID(GameObject gameObject)
        {
            string id = $"{gameObject.name}_{Guid.NewGuid().ToString()}";
            return id;
        }
    }
}