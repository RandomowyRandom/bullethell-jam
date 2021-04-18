using UnityEngine;

public class PlayerRoomTeleporter : MonoBehaviour
{
    private void Start()
    {
        // sub to events
        RoomManager.OnCurrentRoomChanged += TeleportPlayerToRoom;
    }

    private void TeleportPlayerToRoom(Room room, Room oldRoom)
    {
        var pos = room.Position - oldRoom.Position;
        transform.position = room.GetPositionAtDirection(GetReversedDirection(GetVectorDirection(pos)));
    }
    
    private Direction GetVectorDirection(Vector2 vector2)
    {
        if (vector2 == Vector2.up)
            return Direction.Top;
        else if (vector2 == Vector2.down)
            return Direction.Bottom;
        else if (vector2 == Vector2.left)
            return Direction.Left;
        else if (vector2 == Vector2.right)
            return Direction.Right;

        return Direction.Top;
    }

    private Direction GetReversedDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Bottom:
                return Direction.Top;
            case Direction.Top:
                return Direction.Bottom;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
        }
        
        return default;
    }
}