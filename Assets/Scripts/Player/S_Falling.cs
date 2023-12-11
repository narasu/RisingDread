using System;

namespace Player
{
    public class S_Falling : AbstractState
    {
        private Action updateVelocity, applyVelocity, handleWalking, handleGravity;
        private Func<bool> checkIsGrounded;
        public S_Falling(Scratchpad _ownerData, StateMachine _ownerStateMachine) : base(_ownerData, _ownerStateMachine)
        {
            updateVelocity = OwnerData.Read<Action>(Strings.UpdateVelocity);
            applyVelocity = OwnerData.Read<Action>(Strings.ApplyVelocity);
            handleWalking = OwnerData.Read<Action>(Strings.HandleWalking);
            handleGravity = OwnerData.Read<Action>(Strings.HandleGravity);
            checkIsGrounded = OwnerData.Read<Func<bool>>(Strings.CheckIsGrounded);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            updateVelocity.Invoke();
            handleWalking.Invoke();
            handleGravity.Invoke();
            bool isGrounded = checkIsGrounded.Invoke();
            if (isGrounded)
            {
                OwnerStateMachine.SwitchState(typeof(S_Grounded));
            }
            applyVelocity.Invoke();
        }
    }
}