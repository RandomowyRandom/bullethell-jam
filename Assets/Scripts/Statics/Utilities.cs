using UnityEngine;

public static class Utilities
{
    private static Camera _camera;
    private static bool _cameraCached;
    
    public static Vector3 GetMousePosition()
    {
        if (!_cameraCached)
        {
            _camera = Camera.main;
            _cameraCached = true;
        }
        
        var worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(worldPosition.x,worldPosition.y, 0);
    }
}