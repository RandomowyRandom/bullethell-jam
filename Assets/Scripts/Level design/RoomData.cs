using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Room Data")]
public class RoomData : ScriptableObject
{
    [Header("This file SHOULD NOT be changed manually, level creator is made for this.")]
    [Space]
    [SerializeField] private RoomType _roomType;
    [SerializeField] private List<LevelEntity> _entities = new List<LevelEntity>();
    [SerializeField] private List<Direction> _openRoomDoors = new List<Direction>();
    [SerializeField] private TileBase[] _levelTilemap;
    
    public List<LevelEntity> Entities
    {
        get => _entities;
        set => _entities = value;
    }
    public TileBase[] LevelTilemap
    {
        get => _levelTilemap;
        set => _levelTilemap = value;
    }
    public List<Direction> OpenRoomDoors
    {
        get => _openRoomDoors;
        set => _openRoomDoors = value;
    }
    
    public RoomType RoomType
    {
        get => _roomType;
        set => _roomType = value;
    }
}
