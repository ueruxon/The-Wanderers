using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI
{
    public abstract class Consideration : ScriptableObject
    {
        [TextArea][SerializeField] protected string Description;
        [SerializeField] protected AnimationCurve ResponseCurve;

        [SerializeField][Range(0f, 1f)] protected float TestValue = 0;
        
        private float _score;

        public float Score
        {
            get => _score;
            set => _score = Mathf.Clamp01(value);
        }

        public virtual void Init()
        {
            Score = TestValue;
        }

        public abstract float GetScore(IContextProvider contextProvider);
    }
}