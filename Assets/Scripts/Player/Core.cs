using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Core : MonoBehaviour, IStateRunner
    {
        [SerializeField] private PlayerData PlayerDataAsset;
        [SerializeField] private TriggerCheck GroundedTrigger;
        [SerializeField] private Camera FirstPersonCamera;
        [SerializeField] private GameObject[] Arms;
        [SerializeField] private TriggerCheck LeftHand;
        [SerializeField] private TriggerCheck RightHand;
        
        public Scratchpad Pad { get; private set; } = new();
        
        private Rigidbody rb;
        private MovementHandler movementHandler;
        private ClimbingHandler climbingHandler;
        private StateMachine fsm = new();
        private InputHandler inputHandler = new();
        private CameraHandler cameraHandler;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            
            // ObjectData.Write(Strings.InputHandler, inputHandler);
            // ObjectData.Write(Strings.PlayerData, PlayerDataAsset);
            // ObjectData.Write(Strings.GroundedTrigger, GroundedTrigger);

            movementHandler = new MovementHandler(PlayerDataAsset, Pad, inputHandler, rb, GroundedTrigger);
            climbingHandler = new ClimbingHandler(Pad, inputHandler, Arms, LeftHand, RightHand);
            cameraHandler = new CameraHandler(PlayerDataAsset, inputHandler, FirstPersonCamera, rb);
            Utility.LockCursor();
        }

        private void Start()
        {
            fsm.AddState(new S_Grounded(Pad, fsm));
            fsm.AddState(new S_Jumping(Pad, fsm));
            fsm.AddState(new S_Falling(Pad, fsm));
            fsm.AddState(new S_Hanging(Pad, fsm));
            fsm.SwitchState(typeof(S_Grounded));
        }

        private void Update()
        {
            inputHandler.ReadInput();
            fsm.Update();
            cameraHandler.MouseLook();
        }

        private void FixedUpdate()
        {
            cameraHandler.RotateBody();
            fsm.FixedUpdate();
            inputHandler.ConsumeInput();
        }
    }
}