using System.Collections.Generic;
using System.Linq;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI
{
    public class AIBrain : MonoBehaviour
    {
        [SerializeField] private Transform _actionsContainer;

        private List<UtilityAction> _actions;
        private IContextProvider _contextProvider;
        private AIPlanner _aiPlanner;
        
        private UtilityAction _currentAction;
        public UtilityAction CurrentAction => _currentAction;

        private UtilityAction _prevAction;

        public void Init(AIPlanner aiPlanner, IContextProvider contextProvider)
        {
            _actions = _actionsContainer.GetComponentsInChildren<UtilityAction>().ToList();
            _aiPlanner = aiPlanner;
            _contextProvider = contextProvider;

            foreach (UtilityAction utilityAction in _actions) 
                utilityAction.Init(_aiPlanner);
        }

        public void Decide()
        {
            if (_actions.Count == 0)
                return;
            
            if (_currentAction is not null)
            {
                ActionStatus actionStatus = _currentAction.GetActionStatus();

                switch (actionStatus)
                {
                    case ActionStatus.Running:
                        _currentAction.Execute(_contextProvider);
                        break;
                    case ActionStatus.Failed:
                        _currentAction.OnFailed(_contextProvider);
                        break;
                    case ActionStatus.Completed:
                        //SetAction(null);
                        _currentAction.OnCompleted(_contextProvider);
                        break;
                }
            }

            UtilityAction bestAction = DecideBestAction();
            
            if (bestAction.IsActive())
                SetAction(bestAction);
        }

        // Решаем, какое действие сейчас наилучшее
        private UtilityAction DecideBestAction()
        {
            if (_actions.Count == 1)
                return _actions[0];
            
            float bestScore = 0;
            int bestActionIndex = 0;

            for (int i = 0; i < _actions.Count; i++)
            {
                float scoreAction = _actions[i].ScoreAction(_contextProvider);
                
                if (scoreAction > bestScore)
                {
                    bestActionIndex = i;
                    bestScore = _actions[i].Score;
                }
            }

            return _actions[bestActionIndex];
        }

        private void SetAction(UtilityAction utilityAction)
        {
            if (_currentAction is null)
                _currentAction = utilityAction;
            
            if (!Equals(_currentAction, utilityAction))
            {
                _currentAction.OnExit(_contextProvider);
                _currentAction = utilityAction;
                _currentAction.OnEnter(_contextProvider);
            }
        }
    }
}