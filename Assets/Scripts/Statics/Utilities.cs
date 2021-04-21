using UnityEngine;

public static class Utilities
{
    
    public static Vector3 GetMousePosition()
    {
        var worldPosition = Extensions.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(worldPosition.x,worldPosition.y, 0);
    }
}