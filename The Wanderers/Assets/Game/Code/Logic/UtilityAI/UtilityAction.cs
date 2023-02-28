using System.Collections.Generic;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI
{
    public abstract class UtilityAction : MonoBehaviour
    {
        [SerializeField] private string _actionName;
        [SerializeField, Range(0f, 1f)] private float _weight;

        [SerializeField] private List<Consideration> _considerationsTemplates;
        [SerializeField] protected ActionStatus CurrentActionStatus;
        
        private AIPlanner _aIPlanner;
        
        private List<Consideration> _actionConsiderations;

        private float _score;
        public float Score
        {
            get => _score;
            private set => _score = Mathf.Clamp01(value);
        }

        // для инспектора
        public float Weight => _weight;
        public float TotalConsiderationScore { get; private set; }

        public void Init(AIPlanner aiPlanner)
        {
            _aIPlanner = aiPlanner;
            _aIPlanner.TaskReceived += OnTaskReceived;

            _actionConsiderations = new List<Consideration>();

            foreach (Consideration templateObject in _considerationsTemplates)
            {
                Consideration consideration = Instantiate(templateObject);
                consideration.Init();
                consideration.name = templateObject.name;
                
                _actionConsiderations.Add(consideration);
            }
        }

        protected virtual void OnTaskReceived(IContextProvider contextProvider) { }

        protected void CompleteTask() => 
            _aIPlanner.CompleteGlobalTask();

        public ActionStatus GetActionStatus() => 
            CurrentActionStatus;

        public List<Consideration> GetConsiderations() => 
            _actionConsiderations;

        public virtual void OnEnter(IContextProvider contextProvider) =>
            CurrentActionStatus = ActionStatus.Running;
        
        public virtual void OnFailed(IContextProvider contextProvider) => 
            CurrentActionStatus = ActionStatus.Failed;
        
        public virtual void OnCompleted(IContextProvider contextProvider) => 
            CurrentActionStatus = ActionStatus.Completed;
        
        public virtual void OnExit(IContextProvider contextProvider) => 
            CurrentActionStatus = ActionStatus.Completed;

        protected virtual void MoveTo(AIContext context, Vector3 target) { }

        public abstract void Execute(IContextProvider contextProvider);
        
        public bool IsActive() => Score > 0f;

        public virtual float ScoreAction(IContextProvider contextProvider)
        {
            float actionScore = 1f;
            
            // собираем все значения из выбора решений
            for (int i = 0; i < _actionConsiderations.Count; i++)
            {
                float considerationScore = _actionConsiderations[i].GetScore(contextProvider);
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
            float compensationFactor = 1f - (1f / _actionConsiderations.Count);
            float modificationFactor = (1f - originalScore) * compensationFactor;
            float mediateScore = originalScore + (modificationFactor * originalScore);
            // добавляем вес
            float weight = _weight == 0 ? 1 : _weight;
            float totalScore = mediateScore * weight ;

            Score = totalScore;

            return Score;
        }
    }
}
