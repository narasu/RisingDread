using System;
using UnityEngine;

namespace Player
{
    public class InputHandler
    {
        
        public Vector2 MovementVector { get; private set; }
        public float MouseX { get; private set; }
        public float MouseY { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool JumpReleased { get; private set; }
        public bool HangPressed { get; private set; }
        public bool HangReleased { get; private set; }
        public bool DropPressed { get; private set; }
        public bool DropReleased { get; private set; }

        public void ReadInput()
        {
            MovementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            MouseX = Input.GetAxis("Mouse X");
            MouseY = Input.GetAxis("Mouse Y");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                JumpReleased = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                HangPressed = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                HangReleased = true;
            }

            if (Input.GetMouseButtonDown(1))
            {
                DropPressed = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                DropReleased = true;
            }
            DropReleased = Input.GetMouseButtonUp(1);
        }

        public void ConsumeInput()
        {
            JumpPressed = false;
            JumpReleased = false;
            HangPressed = false;
            HangReleased = false;
            DropPressed = false;
            DropReleased = false;
        }

    }

}