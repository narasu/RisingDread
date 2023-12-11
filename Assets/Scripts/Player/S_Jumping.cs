using System;

namespace Player
{
    public class S_Jumping : AbstractState
    {
        private Action updateVelocity, applyVelocity, handleWalking, handleGravity, handleJumpRelease;
        private Func<bool> getIsFalling;

        public S_Jumping(Scratchpad _ownerData, StateMachine _ownerStateMachine) : base(_ownerData, _ownerStateMachine)
        {
            updateVelocity = OwnerData.Read<Action>(Strings.UpdateVelocity);
            applyVelocity = OwnerData.Read<Action>(Strings.ApplyVelocity);
            handleWalking = OwnerData.Read<Action>(Strings.HandleWalking);
            handleGravity = OwnerData.Read<Action>(Strings.HandleGravity);
            handleJumpRelease = OwnerData.Read<Action>(Strings.HandleJumpRelease);
            getIsFalling = OwnerData.Read<Func<bool>>(Strings.GetIsFalling);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            updateVelocity.Invoke();
            handleJumpRelease.Invoke();
            handleWalking.Invoke();
            handleGravity.Invoke();
            
            bool isFalling = getIsFalling.Invoke();
            if (isFalling)
            {
                OwnerStateMachine.SwitchState(typeof(S_Falling));
            }
            
            applyVelocity.Invoke();
        }
    }
}