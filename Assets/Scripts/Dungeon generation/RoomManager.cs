using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static event Action<Room, Room> OnCurrentRoomChanged;
    
    private static List<Room> _rooms = new List<Room>();
    private static Room _currentRoom;

    private List<Entity> _currentRoomEntities = new List<Entity>();

    private void Start()
    {
        // sun to events
        OnCurrentRoomChanged += OnOnCurrentRoomChanged;
        Entity.OnEntityDied += EntityOnOnEntityDied;
    }

    public static void AddRoom(Room room)
    {
        _rooms.Add(room);
    }
    
    public static Room GetRoom(Vector2 position)
    {
        return _rooms.Find(r => r.Position == position);
    }

    public static Room GetCurrentRoom()
    {
        if (_currentRoom == null)
            _currentRoom = GetRoom(new Vector2(15, 15));
        
        return _currentRoom;
    }

    public static void SetCurrentRoom(Room room)
    {
        OnCurrentRoomChanged?.Invoke(room, GetCurrentRoom());
        _currentRoom = room;
    }
    
    private void OnOnCurrentRoomChanged(Room newRoom, Room oldRoom)
    {
        if (newRoom.EntitySpawners.Any())
        {
            _currentRoomEntities.Clear();
            
            foreach (var spawner in newRoom.EntitySpawners)
            {
                spawner.Spawn(out var entity);
                _currentRoomEntities.Add(entity);
            }

            newRoom.EntitySpawners.Clear();
            
            foreach (var door in newRoom.Doors.Values)
            {
                door.SetLock(true);
            }
        }
    }
    
    private void EntityOnOnEntityDied(Entity entity)
    {
        _currentRoomEntities.Remove(entity);

        if (!_currentRoomEntities.Any())
        {
            foreach (var door in GetCurrentRoom().Doors.Values)
            {
                door.SetLock(false);
            }
        }
    }
}
