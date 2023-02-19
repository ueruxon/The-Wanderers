using Game.Code.Common;
using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class InteractionAction : UtilityAction
    {
        // потом поменяем
        private float _currentHitTimer = 0f;
        private float _hitCooldown = 2;
        
        private IInteractable _interactable;
        
        public override void OnEnter(AIContext context)
        {
            base.OnEnter(context);
            
            _currentHitTimer = _hitCooldown;
            
            ICommand command = context.ActionCommand;
            
            // для теста
            //context.GetAnimatorController().SetBool("IsWalking", false);

            if (command is ChopTreeCommand chopTreeCommand) 
                _interactable = chopTreeCommand.ResourceNode;
        }

        public override void Execute(AIContext context)
        {
            _currentHitTimer -= Time.deltaTime;

            if (_currentHitTimer < 0f)
            {
                // убрать аниматор
                context.GetAnimatorController().SetTrigger("Hit");

                _currentHitTimer = _hitCooldown;
                _interactable.Interact();
            }


            if (_interactable.IsActive() == false)
            {
                _currentHitTimer = _hitCooldown;

                CompleteTask();
                CurrentActionStatus = ActionStatus.Completed;
            }
        }
    }
}