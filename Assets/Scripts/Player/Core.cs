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
        
        public Scratchpad ObjectData { get; private set; } = new();
        
        private Rigidbody rb;
        private MovementHandler movementHandler;
        private StateMachine fsm = new();
        private InputHandler inputHandler = new();
        private CameraLook cameraLook;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            
            ObjectData.Write(Strings.InputHandler, inputHandler);
            ObjectData.Write(Strings.PlayerData, PlayerDataAsset);
            ObjectData.Write(Strings.GroundedTrigger, GroundedTrigger);

            movementHandler = new MovementHandler(ObjectData, inputHandler, rb);
            cameraLook = new CameraLook(FirstPersonCamera, rb, inputHandler);
        }

        private void Start()
        {
            fsm.AddState(new S_Grounded(ObjectData, fsm));
            fsm.AddState(new S_Jumping(ObjectData, fsm));
            fsm.AddState(new S_Falling(ObjectData, fsm));
            fsm.SwitchState(typeof(S_Grounded));
        }

        private void Update()
        {
            fsm.Update();
            inputHandler.ReadInput();
        }

        private void FixedUpdate()
        {
            fsm.FixedUpdate();
            cameraLook.HandleCamera();
            inputHandler.ConsumeInput();
        }
    }
}