using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Player Data", order = 1)]
public class PlayerData : ScriptableObject
{
    public float WalkSpeed, RunSpeed, MinJumpHeight, MaxJumpHeight, JumpDuration, FallDuration, CoyoteTime, JumpBufferDuration, HorizontalInertia;
}
