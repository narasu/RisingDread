﻿using System;

namespace Player
{
    public class S_Falling : AbstractState
    {
        private MovementFunctions movementFunctions;
        private ClimbingFunctions climbingFunctions;
        private CameraFunctions cameraFunctions;
        public S_Falling(Scratchpad _ownerData, StateMachine _ownerStateMachine) : base(_ownerData, _ownerStateMachine)
        {
            movementFunctions = OwnerData.Read<MovementFunctions>(Strings.MovementFunctions);
            climbingFunctions = OwnerData.Read<ClimbingFunctions>(Strings.ClimbingFunctions);
            cameraFunctions = OwnerData.Read<CameraFunctions>(Strings.CameraFunctions);
        }
        
        public override void OnUpdate()
        {
            cameraFunctions.MouseLook.Invoke();
        }

        public override void OnFixedUpdate()
        {
            cameraFunctions.SyncBodyRotation.Invoke();
            movementFunctions.ReadVelocity.Invoke();
            movementFunctions.HandleWalking.Invoke();
            movementFunctions.HandleGravity.Invoke();
            bool isGrounded = movementFunctions.CheckIsGrounded.Invoke();
            if (isGrounded)
            {
                OwnerStateMachine.SwitchState(typeof(S_Grounded));
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