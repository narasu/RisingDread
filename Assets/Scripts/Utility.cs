using UnityEngine;

public static class Utility
{
    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public static bool IsAngleInBounds(float _angle, float _min, float _max) {
        float start = (_min + _max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((_angle - start) / 360) * 360;
        return _angle > _min + floor && _angle < _max + floor;
    }
    
    public static float ClampAngle(float _angle, float _min, float _max) {
        float start = (_min + _max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((_angle - start) / 360) * 360;
        return Mathf.Clamp(_angle, _min + floor, _max + floor);
    }
}