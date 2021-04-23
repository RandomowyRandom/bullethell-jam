using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour
{
    public static event Action<Room, Room> OnCurrentRoomChanged;

    [SerializeField] private List<GameObject> _pickups;
    
    private List<Room> _rooms = new List<Room>();
    private Room _currentRoom;

    private List<Entity> _currentRoomEntities = new List<Entity>();

    private void Start()
    {
        // sub to events
        OnCurrentRoomChanged += OnOnCurrentRoomChanged;
        Entity.OnEntityDied += EntityOnOnEntityDied;
    }

    public void AddRoom(Room room)
    {
        _rooms.Add(room);
    }
    
    public Room GetRoom(Vector2 position)
    {
        return _rooms.Find(r => r.Position == position);
    }

    public Room GetCurrentRoom()
    {
        if (_currentRoom == null)
            _currentRoom = GetRoom(new Vector2(15, 15));
        
        return _currentRoom;
    }

    public void SetCurrentRoom(Room room)
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

            if (IsEntity(_currentRoomEntities))
            {
                foreach (var door in newRoom.Doors.Values)
                {
                    door.SetLock(true);
                }
            }
        }
    }

    private void SpawnRandomPickup()
    {
        var position = (Vector2)(PlayerVitalStats.Instance.transform.position + Random.onUnitSphere);
        Instantiate(_pickups.GetRandomElement(), position, Quaternion.identity);
    }
    
    private void EntityOnOnEntityDied(Entity entity)
    {
        _currentRoomEntities.Remove(entity);

        if (!IsEntity(_currentRoomEntities))
        {
            foreach (var door in GetCurrentRoom().Doors.Values)
            {
                door.SetLock(false);
            }

            if (Random.value < .9f)
            {
                SpawnRandomPickup();
            }
        }
    }

    private bool IsEntity(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.IsEnemy)
                return true;
        }

        return false;
    }
    
    private void OnDestroy()
    {
        OnCurrentRoomChanged -= OnOnCurrentRoomChanged;
        Entity.OnEntityDied -= EntityOnOnEntityDied;
    }
}
