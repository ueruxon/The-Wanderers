using Game.Code.Commander;
using Game.Code.Logic.UtilityAI;
using Game.Code.Services.UnitTask;
using UnityEditor;
using UnityEngine;

namespace Game.Code.Editor
{
    [CustomEditor(typeof(AIPlanner))]
    public class AIPlannerInspector : UnityEditor.Editor
    {
        private AIPlanner _aiPlanner;
        
        private void OnEnable()
        {
            _aiPlanner = (AIPlanner)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UnitTask task = _aiPlanner.CurrentTask;
            
            EditorGUILayout.Space(4f);
            EditorGUILayout.Space(4f);

            if (task is not null)
            {
                ICommand command = task.GetCommand();
                float distanceToTarget = Vector3.Distance(_aiPlanner.transform.position, command.Target.position);
                
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Task Status:", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(task.GetTaskStatus().ToString(), GUILayout.Width(200f));
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Command:", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(command.GetType().Name, GUILayout.Width(200f));
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Target object:", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(command.Target.name, GUILayout.Width(200f));
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Goal:", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(command.Goal is not null ? command.Goal.name : "", GUILayout.Width(200f));
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Distance To:", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(distanceToTarget.ToString("0.00"), GUILayout.Width(200f));
                GUILayout.EndHorizontal();
            }
            
        }
    }
}