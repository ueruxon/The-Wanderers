using UnityEngine;
using UnityEngine.AI;

namespace Game.Code.Logic.Units
{
    public abstract class MovementSystemBase : MonoBehaviour
    {
        [SerializeField] protected NavMeshAgent NavAgent;

        public abstract void Init(MovementProperties movementProperties);

        public abstract void CalculatePath(Vector3 targetPosition);
        
        public abstract bool ReachedDestination();

        public virtual float GetRemainingDistance() => 
            NavAgent.remainingDistance;

        public abstract void SetDestination(Vector3 destination);
        
        public abstract void Stop();
    }
}