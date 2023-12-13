using System;

namespace Player
{
    public class S_Jumping : AbstractState
    {
        private MovementFunctions movementFunctions;
        private ClimbingFunctions climbingFunctions;

        public S_Jumping(Scratchpad _ownerData, StateMachine _ownerStateMachine) : base(_ownerData, _ownerStateMachine)
        {
            movementFunctions = OwnerData.Read<MovementFunctions>(Strings.MovementFunctions);
            climbingFunctions = OwnerData.Read<ClimbingFunctions>(Strings.ClimbingFunctions);
        }

        public override void OnFixedUpdate()
        {
            movementFunctions.ReadVelocity.Invoke();
            movementFunctions.HandleJumpRelease.Invoke();
            movementFunctions.HandleWalking.Invoke();
            movementFunctions.HandleGravity.Invoke();
            
            bool isFalling = movementFunctions.GetIsFalling.Invoke();
            if (isFalling)
            {
                OwnerStateMachine.SwitchState(typeof(S_Falling));
            }
            
            movementFunctions.ApplyVelocity.Invoke();
            
            climbingFunctions.HandleClimbPressed.Invoke();
            climbingFunctions.HandleClimbReleased.Invoke();
            
            bool isHanging = climbingFunctions.TryGrabLedge.Invoke();
            if (isHanging)
            {
                OwnerStateMachine.SwitchState(typeof(S_Hanging));
            }
        }
    }
}