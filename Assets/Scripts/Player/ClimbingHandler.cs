namespace Player
{
    public class ClimbingHandler
    {
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