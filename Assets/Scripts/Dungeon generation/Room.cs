using System;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private Vector2 _position;
    private List<Direction> _openDoors;
    private List<EntitySpawner> _entitySpawners;
    private Dictionary<Direction, Door> _doors = new Dictionary<Direction, Door>();

    public Dictionary<Direction, Door> Doors => _doors;
    public Vector2 Position => _position;
    public List<Direction> OpenDoors => _openDoors;
    public List<EntitySpawner> EntitySpawners => _entitySpawners;

    public Room(Vector2 position, List<Direction> directions, List<EntitySpawner> spawners)
    {
        _entitySpawners = spawners;
        _position = position;
        _openDoors = directions;
    }
    
    public Vector2 GetPositionAtDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Top:
                return GetBorderPosition((int) _position.x, (int) _position.y) + new Vector2(9, 8.5f);
            case Direction.Bottom:
                return GetBorderPosition((int) _position.x, (int) _position.y) + new Vector2(9, 1.5f);
            case Direction.Left:
                return GetBorderPosition((int) _position.x, (int) _position.y) + new Vector2(1.5f, 5);
            case Direction.Right:
                return GetBorderPosition((int) _position.x, (int) _position.y) + new Vector2(16.5f, 5);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
    
    private Vector2 GetBorderPosition(int i, int j)
    {
        return new Vector2(i * 18 - 1, j * 10 - 1);
    }
}