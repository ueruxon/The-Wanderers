using UnityEngine;
using UnityEngine.AI;

namespace Game.Code.Logic.Units
{
    class NormalMovementSystem : MovementSystemBase
    {
        public override void Init(MovementProperties movementProps)
        {
            NavAgent.speed = movementProps.Speed;
            NavAgent.acceleration = movementProps.Acceleration;
            NavAgent.angularSpeed = movementProps.AngularSpeed;
            NavAgent.stoppingDistance = movementProps.StoppingDistance;
        }
        
        public override void CalculatePath(Vector3 targetPosition) => 
            NavAgent.SetDestination(targetPosition);

        public override bool ReachedDestination()
        {
            if (NavAgent.pathPending == false)
            {
                if (NavAgent.remainingDistance <= NavAgent.stoppingDistance + .5f)
                {
                    if (NavAgent.hasPath == false || NavAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }

            return false;
            
            // if (NavAgent.pathPending)
            //     return false;
            //
            // Debug.Log($"remaining: {NavAgent.remainingDistance}, stopping: {NavAgent.stoppingDistance}, {NavAgent.remainingDistance < NavAgent.stoppingDistance}");
            //
            // if (NavAgent.remainingDistance < NavAgent.stoppingDistance)
            //     return false;
            //
            // return NavAgent.hasPath == false || NavAgent.velocity.sqrMagnitude == 0f;
        }

        public override void SetDestination(Vector3 destination) => 
            NavAgent.SetDestination(destination);
        

        public override void Stop() => 
            NavAgent.ResetPath();
    }
}