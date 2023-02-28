using System.Collections.Generic;
using System.Linq;
using Game.Code.Data.StaticData.ResourceNode;
using Game.Code.Logic.ResourcesLogic;
using UnityEditor;
using UnityEngine;

namespace Game.Code.Editor
{
    [CustomEditor(typeof(ResourceSpawnersData))]
    public class ResourceSpawnerDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            ResourceSpawnersData levelData = (ResourceSpawnersData)target;

            if (GUILayout.Button("Collect"))
            {
                levelData.ResourceSpawners =
                    FindObjectsOfType<SpawnerMarker>()
                        .Select(x =>
                            new SpawnerData(x.ResourceType, x.transform.position, x.transform))
                        .ToList();
            }

            EditorUtility.SetDirty(target);
        }
    }
}