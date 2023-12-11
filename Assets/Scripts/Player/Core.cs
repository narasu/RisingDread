using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Core : MonoBehaviour, IStateRunner
    {
        [SerializeField] private PlayerData PlayerDataAsset;
        [SerializeField] private TriggerCheck GroundedCheck;
        
        private Rigidbody rb;
        private float hMovementSpeed, jumpGravity, fallGravity, minJumpVelocity, maxJumpVelocity, currentCoyoteTime;
        public Scratchpad ObjectData { get; private set; } = new();
        private StateMachine fsm = new();
        private InputHandler inputHandler = new();

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            jumpGravity = 2f * PlayerDataAsset.MaxJumpHeight / Mathf.Pow(PlayerDataAsset.JumpDuration, 2);
            fallGravity = PlayerDataAsset.MaxJumpHeight / Mathf.Pow(PlayerDataAsset.FallDuration, 2);
            minJumpVelocity = Mathf.Sqrt(2 * jumpGravity * PlayerDataAsset.MinJumpHeight);
            maxJumpVelocity = Mathf.Sqrt(2 * jumpGravity * PlayerDataAsset.MaxJumpHeight);
            
            fsm.AddState(new S_Grounded(ObjectData, fsm));
            fsm.AddState(new S_Jumping(ObjectData, fsm));
            fsm.SwitchState(typeof(S_Grounded));
        }

        private void Update()
        {
            fsm.Update();
        }

        private void FixedUpdate()
        {
            fsm.FixedUpdate();
        }
    }
}