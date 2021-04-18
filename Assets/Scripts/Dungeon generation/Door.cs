﻿using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Vector2 _doorDestination;
    private bool _isLocked;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_isLocked) return;
        
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            var newRoom = RoomManager.GetRoom(RoomManager.GetCurrentRoom().Position + _doorDestination);
            RoomManager.SetCurrentRoom(newRoom);
        }
    }

    public Vector2 DoorDestination
    {
        get => _doorDestination;
        set => _doorDestination = value;
    }

    public void SetLock(bool value)
    {
        _isLocked = value;
    }
}