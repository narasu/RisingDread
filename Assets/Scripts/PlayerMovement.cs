using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed, runSpeed, minJumpHeight, maxJumpHeight, jumpDuration, fallDuration, coyoteTime, jumpBufferDuration, horizontalInertia;
    public TriggerCheck groundedCheck;
    
    private float hMovementSpeed, jumpGravity, fallGravity, minJumpVelocity, maxJumpVelocity, currentCoyoteTime;
    private Rigidbody rb;
    private Vector3 velocity;

    private bool jumpInputPressed, JumpInputReleased, isJumping;

    public Camera cam;

    private Vector2 inputVector;
    private Vector3 movementVector;
    private float xAngle, yAngle;
    public float mouseSensitivity = 120.0f;
    private float mouseX, mouseY;

    public GameObject[] arms;
    private bool armExtendInputPressed, armExtendInputReleased, armsExtended, isHanging;
    public TriggerCheck leftHandCheck, rightHandCheck;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpGravity = 2f * maxJumpHeight / Mathf.Pow(jumpDuration, 2);
        fallGravity = maxJumpHeight / Mathf.Pow(fallDuration, 2);
        minJumpVelocity = Mathf.Sqrt(2 * jumpGravity * minJumpHeight);
        maxJumpVelocity = Mathf.Sqrt(2 * jumpGravity * maxJumpHeight);
        Utility.LockCursor();
    }
    
    private void Update()
    {
        // register all input calls and values
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInputPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            JumpInputReleased = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            armExtendInputPressed = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            armExtendInputReleased = true;
        }
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        
        HandleCameraRotation();
        HandleHorizontalMovement();
        HandleGravity();
        UpdateGrounded();
        HandleHanging();
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
        if (isHanging)
        {
            return;
        }
        hMovementSpeed = walkSpeed;
        Vector3 relativeInput = transform.forward * inputVector.y + transform.right * inputVector.x;
        Vector3 newSpeed = Vector3.Lerp(velocity, relativeInput * hMovementSpeed, horizontalInertia);
        velocity = new Vector3(newSpeed.x, velocity.y, newSpeed.z);
    }

    private void HandleJumping()
    {
        if (jumpInputPressed)
        {
            jumpInputPressed = false;
            if (groundedCheck.HasCollision)
            {
                velocity = new Vector3(velocity.x, maxJumpVelocity, velocity.z);
                isJumping = true;
            }
        }
        
        if (JumpInputReleased)
        {
            JumpInputReleased = false;
            
            if (isJumping && velocity.y > minJumpVelocity)
            {
                velocity = new Vector3(velocity.x, minJumpVelocity, velocity.z);
            }
        }
    }

    private void HandleHanging()
    {
        if (armExtendInputPressed)
        {
            armExtendInputPressed = false;

            if (!isHanging)
            {
                armsExtended = true;
            }
        }

        if (armExtendInputReleased)
        {
            armExtendInputReleased = false;

            if (!isHanging)
            {
                armsExtended = false;
            }
        }

        if (armsExtended)
        {
            foreach (GameObject arm in arms)
            {
                arm.transform.localScale = new Vector3(arm.transform.localScale.x, arm.transform.localScale.y, 1.0f);
                arm.transform.localPosition =
                    new Vector3(arm.transform.localPosition.x, .25f, .25f);
            }
            
            if (!groundedCheck.HasCollision && (leftHandCheck.HasCollision || rightHandCheck.HasCollision))
            {
                isHanging = true;
                velocity = Vector3.zero;
                Debug.Log("grab!");
            }
        }
        else
        {
            foreach (GameObject arm in arms)
            {
                arm.transform.localScale = new Vector3(arm.transform.localScale.x, arm.transform.localScale.y, 0.5f);
                arm.transform.localPosition =
                    new Vector3(arm.transform.localPosition.x, .0f, .0f);
            }
        }
    }

    private void UpdateGrounded()
    {
        if (!groundedCheck.HasCollision)
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
        if (isHanging)
        {
            return;
        }
        
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
