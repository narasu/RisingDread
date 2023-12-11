using System;
using UnityEngine;

namespace Player
{
    public class CameraLook
    {
        private InputHandler inputHandler;
        private Rigidbody rb;
        private float xAngle, yAngle;
        private Camera firstPersonCamera;
        private float mouseSensitivity = 130.0f;

        public CameraLook(Camera _firstPersonCamera, Rigidbody _rb, InputHandler _inputHandler)
        {
            firstPersonCamera = _firstPersonCamera;
            rb = _rb;
            inputHandler = _inputHandler;
        }
        
        public void HandleCamera()
        {
            xAngle += inputHandler.MouseY * mouseSensitivity * Time.deltaTime;
            xAngle = Utility.ClampAngle(xAngle, -80.0f, 90.0f);
            firstPersonCamera.transform.localRotation = Quaternion.Euler(-xAngle, .0f, .0f);
        
            yAngle = inputHandler.MouseX * mouseSensitivity * Time.deltaTime;
            Vector3 currentEuler = rb.rotation.eulerAngles;
            Vector3 newEuler = new(currentEuler.x, currentEuler.y + yAngle, currentEuler.z);
            Quaternion nextRotation = Quaternion.Slerp(Quaternion.Euler(currentEuler), Quaternion.Euler(newEuler), 0.8f);
            rb.MoveRotation(nextRotation);
        }
    }
}