using System;
using UnityEngine;

namespace Player
{
    public class MovementHandler
    {
        private float hMovementSpeed, jumpGravity, fallGravity, minJumpVelocity, maxJumpVelocity, currentCoyoteTime;
        private Scratchpad pad;
        private PlayerData data;
        private InputHandler inputHandler;
        
        private Rigidbody rb;
        private Vector3 velocity;
        private TriggerCheck groundedTrigger;

        public MovementHandler(Scratchpad _pad, InputHandler _inputHandler, Rigidbody _rb)
        {
            pad = _pad;
            inputHandler = _inputHandler;
            rb = _rb;
            
            Action updateVelocity = UpdateVelocity;
            Action applyVelocity = ApplyVelocity;
            Action handleWalking = HandleWalking;
            Action handleGravity = HandleGravity;
            Func<bool> tryJumping = TryJumping;
            Action handleJumpRelease = HandleJumpRelease;
            Func<bool> checkIsGrounded = CheckIsGrounded;
            Func<bool> getIsFalling = GetIsFalling;
            
            pad.Write(Strings.UpdateVelocity, updateVelocity);
            pad.Write(Strings.ApplyVelocity, applyVelocity);
            pad.Write(Strings.HandleWalking, handleWalking);
            pad.Write(Strings.HandleGravity, handleGravity);
            pad.Write(Strings.TryJumping, tryJumping);
            pad.Write(Strings.HandleJumpRelease, handleJumpRelease);
            pad.Write(Strings.CheckIsGrounded, checkIsGrounded);
            pad.Write(Strings.GetIsFalling, getIsFalling);
            
            data = pad.Read<PlayerData>(Strings.PlayerData);
            groundedTrigger = pad.Read<TriggerCheck>(Strings.GroundedTrigger);

            jumpGravity = 2f * data.MaxJumpHeight / Mathf.Pow(data.JumpDuration, 2);
            fallGravity = data.MaxJumpHeight / Mathf.Pow(data.FallDuration, 2);
            minJumpVelocity = Mathf.Sqrt(2 * jumpGravity * data.MinJumpHeight);
            maxJumpVelocity = Mathf.Sqrt(2 * jumpGravity * data.MaxJumpHeight);
        }
        
        private void HandleWalking()
        {
            hMovementSpeed = data.WalkSpeed;
            Vector3 relativeInput = rb.transform.forward * inputHandler.MovementVector.y + rb.transform.right * inputHandler.MovementVector.x;
            Vector3 newSpeed = Vector3.Lerp(velocity, relativeInput * hMovementSpeed, data.HorizontalInertia);
            velocity = new Vector3(newSpeed.x, velocity.y, newSpeed.z);
        }

        private void HandleGravity()
        {
            if (velocity.y >= 0)
            {
                velocity = new Vector3(velocity.x, velocity.y - jumpGravity * Time.fixedDeltaTime, velocity.z);
            }
            else
            {
                velocity = new Vector3(velocity.x, velocity.y - fallGravity * Time.fixedDeltaTime, velocity.z);
            }
        }

        private bool GetIsFalling()
        {
            return velocity.y < .0f;
        }
        
        private bool CheckIsGrounded()
        {
            if (!groundedTrigger.HasCollision)
            {
                return false;
            }
        
            float distance = groundedTrigger.GetNearestDistance();
            if (distance > .0f && velocity.y < .0f)
            {
                rb.MovePosition(rb.transform.position + Vector3.down * distance);
            }
            velocity = new Vector3(velocity.x, .0f, velocity.z);
            return true;
        }
        
        private bool TryJumping()
        {
            if (inputHandler.JumpPressed)
            {
                velocity = new Vector3(velocity.x, maxJumpVelocity, velocity.z);
                return true;
            }

            return false;
        }

        private void HandleJumpRelease()
        {
            if (inputHandler.JumpReleased && velocity.y > minJumpVelocity)
            {
                velocity = new Vector3(velocity.x, minJumpVelocity, velocity.z);
            }
        }

        private void UpdateVelocity() => velocity = rb.velocity;

        private void ApplyVelocity() => rb.velocity = velocity;
    }
}