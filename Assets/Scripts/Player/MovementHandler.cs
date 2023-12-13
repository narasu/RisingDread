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

        public MovementHandler(PlayerData _data, Scratchpad _pad, InputHandler _inputHandler, Rigidbody _rb, TriggerCheck _groundedTrigger)
        {
            pad = _pad;
            inputHandler = _inputHandler;
            rb = _rb;
            data = _data;
            groundedTrigger = _groundedTrigger;

            MovementFunctions movementFunctions = new()
            {
                ReadVelocity = this.ReadVelocity,
                ApplyVelocity = this.ApplyVelocity,
                StopVelocity = this.StopVelocity,
                HandleWalking = this.HandleWalking,
                HandleGravity = this.HandleGravity,
                TryJumping = this.TryJumping,
                HandleJumpRelease = this.HandleJumpRelease,
                CheckIsGrounded = this.CheckIsGrounded,
                GetIsFalling = this.GetIsFalling
            };
            
            pad.Write(Strings.MovementFunctions, movementFunctions);
            

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

        private void ReadVelocity() => velocity = rb.velocity;

        private void ApplyVelocity() => rb.velocity = velocity;

        private void StopVelocity()
        {
            rb.velocity = Vector3.zero;
            velocity = Vector3.zero;
        }
    }
}