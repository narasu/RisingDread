using System;
using UnityEngine;

namespace Player
{
    public class S_Grounded : AbstractState
    {
        private Action getVelocity, applyVelocity, handleWalking, handleGravity;
        private Func<bool> tryJumping, checkIsGrounded;
        public S_Grounded(Scratchpad _ownerData, StateMachine _ownerStateMachine) : base(_ownerData, _ownerStateMachine)
        {
            getVelocity = OwnerData.Read<Action>(Strings.UpdateVelocity);
            applyVelocity = OwnerData.Read<Action>(Strings.ApplyVelocity);
            handleWalking = OwnerData.Read<Action>(Strings.HandleWalking);
            handleGravity = OwnerData.Read<Action>(Strings.HandleGravity);
            tryJumping = OwnerData.Read<Func<bool>>(Strings.TryJumping);
            checkIsGrounded = OwnerData.Read<Func<bool>>(Strings.CheckIsGrounded);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            getVelocity.Invoke();
            
            handleWalking.Invoke();
            handleGravity.Invoke();

            bool isGrounded = checkIsGrounded.Invoke();
            if (!isGrounded)
            {
                OwnerStateMachine.SwitchState(typeof(S_Falling));
            }
            else
            {
                bool isJumping = tryJumping.Invoke();
                if (isJumping)
                {
                    OwnerStateMachine.SwitchState(typeof(S_Jumping));
                }
            }
            
            applyVelocity.Invoke();
        }
    }
}