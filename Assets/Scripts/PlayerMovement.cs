using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed, runSpeed, rotationSpeed, minJumpHeight, maxJumpHeight, jumpDuration, fallDuration, coyoteTime, jumpBufferDuration, horizontalInertia;
    public GroundCheck groundedCheck;
    public GroundCheck hangingCheck;
    
    private float hMovementSpeed, jumpGravity, fallGravity, minJumpVelocity, maxJumpVelocity, currentCoyoteTime;
    private Rigidbody rb;
    private Vector3 velocity;
    private Quaternion rotation;

    private bool jumpQueued, jumpReleaseQueued, isJumping;

    public Camera cam;
    private Transform camTransform;

    private Vector2 inputVector;
    private Vector3 movementVector;
    private float xAngle, yAngle;
    public float mouseSensitivity = 120.0f;
    private float mouseX, mouseY;

    private GameObject arms;
    private bool armExtendQueued, armReleaseQueued, armsExtended, isHanging;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpGravity = 2f * maxJumpHeight / Mathf.Pow(jumpDuration, 2);
        fallGravity = maxJumpHeight / Mathf.Pow(fallDuration, 2);
        minJumpVelocity = Mathf.Sqrt(2 * jumpGravity * minJumpHeight);
        maxJumpVelocity = Mathf.Sqrt(2 * jumpGravity * maxJumpHeight);
        if (cam != null)
        {
            camTransform = cam.transform;
        }
        Utility.LockCursor();
        xAngle = .0f;
    }

    private void QueueJump()
    {
        if (groundedCheck.isGrounded)
        {
            jumpQueued = true;
        }
    }

    private void QueueJumpRelease()
    {
        if (isJumping)
        {
            jumpReleaseQueued = true;
        }
    }

    private void QueueExtendArms()
    {
        armExtendQueued = true;
    }
    
    private void QueueReleaseArms()
    {
        armReleaseQueued = true;
    }
    
    private void Update()
    {
        // register all input calls and values
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        //movementVector = new Vector3(inputVector.x, .0f, inputVector.y).normalized;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            QueueJump();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            QueueJumpRelease();
        }

        if (Input.GetMouseButtonDown(0))
        {
            QueueExtendArms();
        }

        if (Input.GetMouseButtonUp(0))
        {
            QueueReleaseArms();
        }
        
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        
        HandleCameraRotation();
        HandleHorizontalMovement();
        HandleGravity();
        UpdateGrounded();
        HandleJumping();
        
        rb.velocity = velocity;
    }

    private void HandleCameraRotation()
    {
        xAngle += mouseY * mouseSensitivity * Time.deltaTime;
        xAngle = Utility.ClampAngle(xAngle, -80.0f, 90.0f);
        cam.transform.localRotation = Quaternion.Euler(-xAngle, .0f, .0f);
        
        yAngle = mouseX * mouseSensitivity * Time.deltaTime;
        Vector3 currentEuler = rb.rotation.eulerAngles;
        Vector3 newEuler = new(currentEuler.x, currentEuler.y + yAngle, currentEuler.z);
        Quaternion nextRotation = Quaternion.Slerp(Quaternion.Euler(currentEuler), Quaternion.Euler(newEuler), 0.8f);
        rb.MoveRotation(nextRotation);
    }
    
    private void HandleHorizontalMovement()
    {
        hMovementSpeed = walkSpeed;
        Vector3 relativeInput = transform.forward * inputVector.y + transform.right * inputVector.x;
        Vector3 newSpeed = Vector3.Lerp(velocity, relativeInput * hMovementSpeed, horizontalInertia);
        velocity = new Vector3(newSpeed.x, velocity.y, newSpeed.z);
    }

    private void HandleJumping()
    {
        if (jumpQueued && groundedCheck.isGrounded)
        {
            velocity = new Vector3(velocity.x, maxJumpVelocity, velocity.z);
            isJumping = true;
            jumpQueued = false;
        }
        
        if (jumpReleaseQueued)
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity = new Vector3(velocity.x, minJumpVelocity, velocity.z);
            }

            jumpReleaseQueued = false;
        }
    }

    private void HandleHanging()
    {
        if (armExtendQueued)
        {
            armsExtended = true;
            armExtendQueued = false;
        }

        if (armReleaseQueued)
        {
            armsExtended = false;
            armReleaseQueued = false;
        }
        
        if (armsExtended && !isHanging)
        {
            
        }
    }

    private void UpdateGrounded()
    {
        if (!groundedCheck.isGrounded)
        {
            return;
        }
        
        float distance = groundedCheck.GetNearestDistance();
        if (distance > .0f)
        {
            rb.MovePosition(transform.position + Vector3.down * distance);
            if (velocity.y < .0f)
            {
                isJumping = false;
            }
        }

        if (!isJumping)
        {
            velocity = new Vector3(velocity.x, .0f, velocity.z);
        }
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
    
    public bool IsMoving()
    {
        return Mathf.Abs(velocity.sqrMagnitude) > 1.0f;
    }
}
