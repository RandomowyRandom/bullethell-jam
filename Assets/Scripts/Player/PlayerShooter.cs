using System;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Transform _gunHandle;

    private Camera _mainCamera;

    private void Start()
    {
        // cache references
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        var mousePosition = Utilities.GetMousePosition();

        var aimDirection = (mousePosition - transform.position).normalized;
        var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        _gunHandle.eulerAngles = new Vector3(0, 0, angle);

        var localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            // left
            localScale.y = -1;
        }
        else
        {
            // right
            localScale.y = 1;
        }

        _gunHandle.localScale = localScale;
    }
}
