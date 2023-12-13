using System;
using UnityEngine;

namespace Player
{
    public class CameraHandler
    {
        private PlayerData playerData;
        private Camera firstPersonCamera;
        private InputHandler inputHandler;
        private Rigidbody rb;
        private float xAngle, yAngle;
        private float mouseSensitivity;

        public CameraHandler(PlayerData _playerData, InputHandler _inputHandler, Camera _firstPersonCamera, Rigidbody _rb)
        {
            playerData = _playerData;
            mouseSensitivity = playerData.MouseSensitivity;
            firstPersonCamera = _firstPersonCamera;
            inputHandler = _inputHandler;
            rb = _rb;
        }
        
        public void MouseLook()
        {
            xAngle += inputHandler.MouseY * mouseSensitivity * Time.deltaTime;
            xAngle = Utility.ClampAngle(xAngle, -80.0f, 90.0f);
            yAngle = inputHandler.MouseX * mouseSensitivity * Time.deltaTime;

            Vector3 currentEuler = firstPersonCamera.transform.localRotation.eulerAngles;
            firstPersonCamera.transform.localRotation = Quaternion.Euler(-xAngle, currentEuler.y + yAngle, .0f);
        }
        
        public void RotateBody()
        {
            Vector3 rbCurrentEuler = rb.rotation.eulerAngles;
            Vector3 rbNewEuler = new(rbCurrentEuler.x, firstPersonCamera.transform.rotation.eulerAngles.y, rbCurrentEuler.z);
            rb.MoveRotation(Quaternion.Euler(rbNewEuler));
            
            Vector3 camEulerLocal = firstPersonCamera.transform.localRotation.eulerAngles;
            firstPersonCamera.transform.localRotation = Quaternion.Euler(camEulerLocal.x, .0f, .0f);
        }
    }
}