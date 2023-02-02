using System.Collections.Generic;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI
{
    public abstract class UtilityAction : MonoBehaviour
    {
        [SerializeField] private string _actionName;
        [SerializeField, Range(0f, 1f)] private float _weight;

        [SerializeField] private List<Consideration> _considerations;
        [SerializeField] protected ActionStatus CurrentActionStatus;
        
        protected AIPlanner AIPlanner;
        
        private float _score;
        public float Score
        {
            get => _score;
            set => _score = Mathf.Clamp01(value);
        }

        // для инспектора
        public float Weight => _weight;
        public float TotalConsiderationScore { get; private set; }
        
        public void Init(AIPlanner aiPlanner)
        {
            AIPlanner = aiPlanner;
            AIPlanner.TaskReceived += OnTaskReceived;

            foreach (Consideration consideration in _considerations) 
                consideration.Init();
        }

        protected virtual void OnTaskReceived(AIContext context) { }

        public ActionStatus GetActionStatus() => CurrentActionStatus;

        public List<Consideration> GetConsiderations() => _considerations;

        public virtual void OnEnter(AIContext context) =>
            CurrentActionStatus = ActionStatus.Running;
        
        public virtual void OnFailed(AIContext context) => 
            CurrentActionStatus = ActionStatus.Failed;
        
        public virtual void OnCompleted(AIContext context) => 
            CurrentActionStatus = ActionStatus.Completed;

        protected virtual void MoveTo(AIContext context, Vector3 target) { }

        public bool IsActive() => Score > 0f;

        public virtual float ScoreAction(AIContext context)
        {
            float actionScore = 1f;
            
            // собираем все значения из выбора решений
            for (int i = 0; i < _considerations.Count; i++)
            {
                float considerationScore = _considerations[i].GetScore(context);
                actionScore *= considerationScore;

                TotalConsiderationScore = actionScore;

                if (actionScore == 0)
                {
                    Score = 0;
                    return Score;
                }
            }
            
            // усредняем значения
            float originalScore = actionScore;
            float compensationFactor = 1f - (1f / _considerations.Count);
            float modificationFactor = (1f - originalScore) * compensationFactor;
            float mediateScore = originalScore + (modificationFactor * originalScore);
            // добавляем вес
            float weight = _weight == 0 ? 1 : _weight;
            float totalScore = mediateScore * weight ;

            Score = totalScore;

            return Score;
        }

        public abstract void Execute(AIContext context);
    }
}
