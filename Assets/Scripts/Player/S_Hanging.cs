namespace Player
{
    public class S_Hanging : AbstractState
    {
        private MovementFunctions movementFunctions;
        private ClimbingFunctions climbingFunctions;
        public S_Hanging(Scratchpad _ownerData, StateMachine _ownerStateMachine) : base(_ownerData, _ownerStateMachine)
        {
            movementFunctions = OwnerData.Read<MovementFunctions>(Strings.MovementFunctions);
            climbingFunctions = OwnerData.Read<ClimbingFunctions>(Strings.ClimbingFunctions);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            movementFunctions.StopVelocity.Invoke();
        }

        public override void OnFixedUpdate()
        {
            movementFunctions.ReadVelocity.Invoke();

            bool isJumping = movementFunctions.TryJumping.Invoke();
            if (isJumping)
            {
                OwnerStateMachine.SwitchState(typeof(S_Jumping));
            }
            
            movementFunctions.ApplyVelocity.Invoke();

            bool isDropping = climbingFunctions.TryDropFromLedge.Invoke();
            if (isDropping)
            {
                OwnerStateMachine.SwitchState(typeof(S_Falling));
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            climbingFunctions.RetractArms.Invoke();
        }
    }
}