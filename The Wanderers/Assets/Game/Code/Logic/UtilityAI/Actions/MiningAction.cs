using Game.Code.Common;
using Game.Code.Logic.UtilityAI.Commander;
using Game.Code.Logic.UtilityAI.Context;
using UnityEngine;

namespace Game.Code.Logic.UtilityAI.Actions
{
    public class MiningAction : UtilityAction
    {
        // потом поменяем
        private float _currentHitTimer = 0f;
        private float _hitCooldown = 2;
        
        private IInteractable _interactable;
        
        public override void OnEnter(IContextProvider contextProvider)
        {
            base.OnEnter(contextProvider);
            
            _currentHitTimer = _hitCooldown;
            
            ICommand command = contextProvider.GetContext().ActionCommand;
            
            // для теста
            //context.GetAnimatorController().SetBool("IsWalking", false);

            if (command is MiningCommand chopTreeCommand) 
                _interactable = chopTreeCommand.ResourceNode;
        }

        public override void Execute(IContextProvider contextProvider)
        {
            _currentHitTimer -= Time.deltaTime;

            if (_currentHitTimer < 0f)
            {
                // убрать аниматор
                contextProvider
                    .GetContext()
                    .GetAnimatorController()
                    .SetTrigger("Hit");

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