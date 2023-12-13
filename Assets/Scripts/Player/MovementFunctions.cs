using System;

namespace Player
{
    public struct MovementFunctions
    {
        public Action ReadVelocity;
        public Action ApplyVelocity;
        public Action StopVelocity;
        public Action HandleWalking;
        public Action HandleGravity;
        public Func<bool> TryJumping;
        public Action HandleJumpRelease;
        public Func<bool> CheckIsGrounded;
        public Func<bool> GetIsFalling;
        public Action RotateBody;
    }
}