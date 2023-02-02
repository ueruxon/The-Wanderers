using System.Collections.Generic;
using Game.Code.Logic.UtilityAI;
using UnityEngine;
using UnityEditor;

namespace Game.Code.Editor
{
    [CustomEditor(typeof(AIBrain))]
    public class AIBrainInspector : UnityEditor.Editor
    {
        private AIBrain _aiBrain;

        private UtilityAction[] _allActions;
        private bool[] _collapseActions;


        private void OnEnable()
        {
            _aiBrain = (AIBrain) target;
            _allActions = _aiBrain.transform.GetComponentsInChildren<UtilityAction>();

            _collapseActions = new bool[_allActions.Length];
            for (int i = 0; i < _collapseActions.Length; i++)
                _collapseActions[i] = false;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(4f);
            EditorGUILayout.Space(4f);
            EditorGUILayout.Space(4f);
            EditorGUILayout.Space(4f);
            EditorGUILayout.Space(4f);
            EditorGUILayout.Space(4f);
            EditorGUILayout.LabelField($"All Actions: ", EditorStyles.boldLabel);

            for (int i = 0; i < _allActions.Length; i++)
            {
                UtilityAction action = _allActions[i];
                bool isActiveAction = action == _aiBrain.CurrentAction;

                GUILayout.BeginHorizontal();
                GUI.color = isActiveAction ? Color.green : Color.white;
                _collapseActions[i] = EditorGUILayout.Foldout(_collapseActions[i], action.name, true);
                EditorGUILayout.LabelField($"Score: {action.Score}", EditorStyles.boldLabel, GUILayout.Width(100f));
                GUI.color = Color.white;
                GUILayout.EndHorizontal();

                if (_collapseActions[i])
                {
                    string isActive = action.IsActive().ToString();
                    string weight = action.Weight.ToString("0.00");
                    string totalConsiderationScore = action.TotalConsiderationScore.ToString("0.00");
                    string actionStatus = action.GetActionStatus().ToString();
                    List<Consideration> considerations = action.GetConsiderations();
                    
                    GUILayout.BeginVertical("HelpBox");

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"IsActive:", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField(isActive, GUILayout.Width(160f));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Weight:", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField(weight, GUILayout.Width(160f));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Considerations Score:", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField(totalConsiderationScore, GUILayout.Width(160f));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Action Status:", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField(actionStatus, GUILayout.Width(160f));
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    GUILayout.Space(8f);

                    GUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.LabelField($"Considerations:", EditorStyles.boldLabel);

                    for (int j = 0; j < considerations.Count; j++)
                    {
                        Consideration consideration = considerations[j];
                        
                        GUILayout.BeginVertical("HelpBox");
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField($"{consideration.name}", EditorStyles.boldLabel);
                        EditorGUILayout.LabelField($"Score {consideration.Score}");
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }
                    
                    GUILayout.EndVertical();
                }
            }
        }
    }
}