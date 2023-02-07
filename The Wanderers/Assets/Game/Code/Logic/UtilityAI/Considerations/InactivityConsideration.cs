using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Inactivity", menuName = "UtilityAI/Considerations/Inactivity")]
    public class InactivityConsideration : Consideration
    {
        [SerializeField] private int _timeBeforeRest = 10;

        private float _inactivityTimer;

        public override void Init()
        {
            base.Init();

            _inactivityTimer = 0f;
        }

        public override float GetScore(AIContext context)
        {
            if (context.IsGlobalCommand == false) 
                _inactivityTimer += Time.deltaTime;
            else
                _inactivityTimer = 0;
            
            Score = _inactivityTimer > _timeBeforeRest ? 1 : 0f;
            return Score;
        }
    }
}