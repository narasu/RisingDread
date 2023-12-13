using System;
using UnityEngine;

namespace Player
{
    public class CameraHandler
    {
        private PlayerData playerData;
        private Scratchpad pad;
        private Camera firstPersonCamera;
        private InputHandler inputHandler;
        private Rigidbody rb;
        private float xAngle, yAngle;
        private float mouseSensitivity;

        public CameraHandler(PlayerData _playerData, Scratchpad _pad, InputHandler _inputHandler, Camera _firstPersonCamera, Rigidbody _rb)
        {
            playerData = _playerData;
            mouseSensitivity = playerData.MouseSensitivity;
            pad = _pad;
            firstPersonCamera = _firstPersonCamera;
            inputHandler = _inputHandler;
            rb = _rb;

            CameraFunctions cameraFunctions = new()
            {
                MouseLook = this.MouseLook,
                SyncBodyRotation = this.SyncBodyRotation,
                ResetCamera = this.ResetCamera
            };
            
            pad.Write(Strings.CameraFunctions, cameraFunctions);
        }

        private void MouseLook()
        {
            xAngle += inputHandler.MouseY * mouseSensitivity * Time.deltaTime;
            xAngle = Utility.ClampAngle(xAngle, -80.0f, 90.0f);
            yAngle = inputHandler.MouseX * mouseSensitivity * Time.deltaTime;

            Vector3 currentEuler = firstPersonCamera.transform.localRotation.eulerAngles;
            firstPersonCamera.transform.localRotation = Quaternion.Euler(-xAngle, currentEuler.y + yAngle, .0f);
        }

        private void SyncBodyRotation()
        {
            Vector3 rbCurrentEuler = rb.rotation.eulerAngles;
            Vector3 rbNewEuler = new(rbCurrentEuler.x, firstPersonCamera.transform.rotation.eulerAngles.y, rbCurrentEuler.z);
            rb.MoveRotation(Quaternion.Euler(rbNewEuler));
            
            Vector3 camEulerLocal = firstPersonCamera.transform.localRotation.eulerAngles;
            firstPersonCamera.transform.localRotation = Quaternion.Euler(camEulerLocal.x, .0f, .0f);
        }

        private void ResetCamera()
        {
            firstPersonCamera.transform.localRotation = Quaternion.identity;
            xAngle = .0f;
            yAngle = .0f;
        }
    }
}