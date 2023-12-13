using System;

namespace Player
{
    public struct ClimbingFunctions
    {
        public Action HandleClimbPressed;
        public Action HandleClimbReleased;
        public Action ExtendArms;
        public Action RetractArms;
        public Func<bool> TryGrabLedge;
        public Func<bool> TryDropFromLedge;
    }
}