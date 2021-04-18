using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed;
    private Vector2 _endPos = new Vector2(278, 154);
    private void Start()
    {
        // sub to events
        RoomManager.OnCurrentRoomChanged += SetCameraPositionAtRoom;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position, 
            new Vector3(_endPos.x, _endPos.y, -10), 
            Time.deltaTime * _scrollSpeed);
    }

    public void SetCameraPositionAtRoom(Room room, Room oldRoom)
    {
        _endPos = new Vector3(room.Position.x * 18 + 8, room.Position.y * 10 - 1 + 5, -10);
    }
}
