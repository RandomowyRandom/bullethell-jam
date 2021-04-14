using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Room Data")]
public class RoomData : ScriptableObject
{
    [SerializeField] private string _roomName;
    [SerializeField] private List<LevelEntity> _entities = new List<LevelEntity>();
    [SerializeField] private List<Direction> _openRoomDoors = new List<Direction>();
    private TileBase[,] _levelTilemap = new TileBase[16, 8];

    public string RoomName
    {
        get => _roomName;
        set => _roomName = value;
    }
    public List<LevelEntity> Entities
    {
        get => _entities;
        set => _entities = value;
    }
    public TileBase[,] LevelTilemap
    {
        get => _levelTilemap;
        set => _levelTilemap = value;
    }
    public List<Direction> OpenRoomDoors
    {
        get => _openRoomDoors;
        set => _openRoomDoors = value;
    }
}
