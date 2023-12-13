using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Player Data", order = 1)]
public class PlayerData : ScriptableObject
{
    public float WalkSpeed;
    public float RunSpeed;
    public float MinJumpHeight;
    public float MaxJumpHeight;
    public float JumpDuration;
    public float FallDuration;
    public float CoyoteTime;
    public float JumpBufferDuration;
    public float HorizontalInertia;
    public float MouseSensitivity;
}
