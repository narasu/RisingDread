using System;
using UnityEngine;

namespace Player
{
    public class ClimbingHandler
    {
        private InputHandler inputHandler;
        private Scratchpad pad;
        private GameObject[] arms;
        private TriggerCheck leftHand, rightHand;
        private bool armsExtended = false;
        public ClimbingHandler(Scratchpad _pad, InputHandler _inputHandler, GameObject[] _arms, TriggerCheck _leftHand, TriggerCheck _rightHand)
        {
            inputHandler = _inputHandler;
            pad = _pad;
            arms = _arms;
            leftHand = _leftHand;
            rightHand = _rightHand;

            ClimbingFunctions climbingFunctions = new()
            {
                HandleClimbPressed = this.HandleClimbPressed,
                HandleClimbReleased = this.HandleClimbReleased,
                ExtendArms = this.ExtendArms,
                RetractArms = this.RetractArms,
                TryGrabLedge = this.TryGrabLedge,
                TryDropFromLedge = this.TryDropFromLedge
            };
            pad.Write(Strings.ClimbingFunctions, climbingFunctions);
        }

        private void HandleClimbPressed()
        {
            if (inputHandler.ClimbPressed && !armsExtended)
            {
                ExtendArms();
            }
        }

        private void HandleClimbReleased()
        {
            if (inputHandler.ClimbReleased && armsExtended)
            {
                RetractArms();
            }
        }

        private void ExtendArms()
        {
            foreach (GameObject arm in arms)
            {
                arm.transform.localScale = new Vector3(arm.transform.localScale.x, arm.transform.localScale.y, 1.0f);
                arm.transform.localPosition =
                    new Vector3(arm.transform.localPosition.x, .25f, .25f);
            }

            armsExtended = true;
        }

        private void RetractArms()
        {
            foreach (GameObject arm in arms)
            {
                arm.transform.localScale = new Vector3(arm.transform.localScale.x, arm.transform.localScale.y, 0.5f);
                arm.transform.localPosition =
                    new Vector3(arm.transform.localPosition.x, .0f, .0f);
            }

            armsExtended = false;
        }

        private bool TryGrabLedge()
        {
            if (!armsExtended)
            {
                return false;
            }
            
            if (leftHand.HasCollision || rightHand.HasCollision)
            {
                return true;
            }
            
            return false;
        }

        private bool TryDropFromLedge()
        {
            if (inputHandler.DropPressed && armsExtended)
            {
                RetractArms();
                return true;
            }

            return false;
        }
        // private void HandleHanging()
        // {
        //     if (armExtendInputPressed)
        //     {
        //         armExtendInputPressed = false;
        //
        //         if (!isHanging)
        //         {
        //             armsExtended = true;
        //         }
        //     }
        //
        //     if (armExtendInputReleased)
        //     {
        //         armExtendInputReleased = false;
        //
        //         if (!isHanging)
        //         {
        //             armsExtended = false;
        //         }
        //     }
        //
        //     if (armsExtended)
        //     {
        //         foreach (GameObject arm in arms)
        //         {
        //             arm.transform.localScale = new Vector3(arm.transform.localScale.x, arm.transform.localScale.y, 1.0f);
        //             arm.transform.localPosition =
        //                 new Vector3(arm.transform.localPosition.x, .25f, .25f);
        //         }
        //         
        //         if (!groundedCheck.HasCollision && (leftHandCheck.HasCollision || rightHandCheck.HasCollision))
        //         {
        //             isHanging = true;
        //             velocity = Vector3.zero;
        //             Debug.Log("grab!");
        //         }
        //     }
        //     else
        //     {
        //         foreach (GameObject arm in arms)
        //         {
        //             arm.transform.localScale = new Vector3(arm.transform.localScale.x, arm.transform.localScale.y, 0.5f);
        //             arm.transform.localPosition =
        //                 new Vector3(arm.transform.localPosition.x, .0f, .0f);
        //         }
        //     }
        // }
    }
}