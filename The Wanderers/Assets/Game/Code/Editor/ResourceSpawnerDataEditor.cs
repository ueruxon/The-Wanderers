using Game.Code.Data.StaticData.ResourceNodeData;
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
                var spawners = FindObjectsOfType<ResourceNodeSpawner>();

                foreach (ResourceNodeSpawner nodeSpawner in spawners)
                {
                    SpawnerData data = new SpawnerData(
                        nodeSpawner, nodeSpawner.GetResourceType(), nodeSpawner.transform.position);
                    
                    levelData.ResourceSpawners.Add(data);
                }
            }

            EditorUtility.SetDirty(target);
        }
    }
}