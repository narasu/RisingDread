using UnityEngine;

namespace Player
{
    public class InputHandler
    {
        private float mouseSensitivity = 130.0f;
        public InputHandler()
        {
            //TODO: inject mouse sensitivity
        }
        public InputData GetInputData()
        {
            return new InputData
            {
                // register all input calls and values
                MovementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized,
                MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime,
                MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime,
                JumpPressed = Input.GetKeyDown(KeyCode.Space),
                JumpReleased = Input.GetKeyUp(KeyCode.Space),
                HangPressed = Input.GetMouseButtonDown(0),
                HangReleased = Input.GetMouseButtonUp(0),
                DropPressed = Input.GetMouseButtonDown(1),
                DropReleased = Input.GetMouseButtonUp(1)
            };
        }
    }

    public struct InputData
    {
        public Vector2 MovementVector;
        public float MouseX, MouseY;
        public bool JumpPressed, JumpReleased;
        public bool HangPressed, HangReleased;
        public bool DropPressed, DropReleased;
    }
}