using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Room layouts")]
    [SerializeField] private List<RoomData> _roomLayouts;
    [SerializeField] private List<RoomData> _lootRoomLayouts;
    [SerializeField] private List<RoomData> _bossRoomLayouts;
    [SerializeField] private List<RoomData> _startRoomLayouts;
    [Header("Tilemaps")]
    [SerializeField] private Tilemap _borderTilemap;
    [SerializeField] private Tilemap _floorTilemap;
    [SerializeField] private Tilemap _templateTilemap;
    [Header("Map properties")]
    [SerializeField] private MapInfo _mapInfo;
    [SerializeField] private RoomData _border;
    [SerializeField] private TileBase _floorTile;
    [SerializeField] private Door _doorPrefab;
    [Header("Depends")]
    [SerializeField] private EntitySpawner _entitySpawner;
    [SerializeField] private RoomManager _roomManager;
    private DungeonMap _dungeonMap;

    private void Start()
    {
        _dungeonMap = new DungeonMap(_mapInfo);
        ProcessMap(_dungeonMap);
    }

    private void ProcessMap(DungeonMap dungeonMap)
    {
        for (var i = 0; i < 30; i++)
        {
            for (var j = 0; j < 30; j++)
            {
                if (dungeonMap.Map[i, j].RoomType != RoomType.None)
                {
                    var cell = dungeonMap.Map[i, j];
                    
                    // get random room and spawn it
                    var randomRoom = GetRandomRoomAtDirection(cell.RoomDirections, dungeonMap.Map[i, j].RoomType);
                    SpawnRoom(GetRoomPosition(i, j), randomRoom);

                    // spawn borders
                    var borderPosition = GetBorderPosition(i, j);
                    SpawnRoomBorder(borderPosition);
                    
                    // spawn the floor
                    SpawnRoomFloor(GetRoomPosition(i, j));

                    // instantiate entity spawners
                    var spawners = new List<EntitySpawner>();
                    
                    foreach (var entity in randomRoom.Entities)
                    {
                        var spawner = InstantiateEntitySpawner(entity, new Vector2(borderPosition.x, borderPosition.y));
                        spawners.Add(spawner);
                    }
                    
                    // add room to RoomManager
                    var room = new Room(new Vector2(i, j), cell.RoomDirections, spawners);
                    _roomManager.AddRoom(room);
                    
                    // create doors
                    foreach (var cellRoomDirection in cell.RoomDirections)
                    {
                        MakeHoleInRoom(cellRoomDirection, borderPosition);
                        var door = SpawnDoor(cellRoomDirection, borderPosition);
                        _roomManager.GetRoom(new Vector2(i, j)).Doors.Add(cellRoomDirection, door);
                    }
                }
            }
        }
    }

    private Vector3Int GetRoomPosition(int i, int j)
    {
        return new Vector3Int(i * 18, j * 10, 0);
    }

    private Vector3Int GetBorderPosition(int i, int j)
    {
        return new Vector3Int(i * 18 - 1, j * 10 - 1, 0);
    }
    
    private void SpawnRoom(Vector3Int origin, RoomData room)
    {
        var roomLayout = Get2DArray(room.LevelTilemap, 16, 8);

        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                _templateTilemap.SetTile(new Vector3Int(x + origin.x, y + origin.y, 0), roomLayout[x, y]);
            }
        }
    }

    private void SpawnRoomBorder(Vector3Int origin)
    {
        var roomBorder = Get2DArray(_border.LevelTilemap, 18, 10);

        for (int x = 0; x < 18; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if(roomBorder[x, y] != null)
                    _borderTilemap.SetTile(new Vector3Int(x + origin.x, y + origin.y, 0), roomBorder[x, y]);
            }
        }
    }

    private void SpawnRoomFloor(Vector3Int origin)
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                _floorTilemap.SetTile(new Vector3Int(x + origin.x, y + origin.y, 0), _floorTile);
            }
        }
    }
    
    private RoomData GetRandomRoomAtDirection(List<Direction> directions, RoomType roomType)
    {
        var supportedRooms = new List<RoomData>();

        switch (roomType)
        {
            case RoomType.Loot:
                foreach (var layout in _lootRoomLayouts)
                {
                    var firstNotSecond = directions.Except(layout.OpenRoomDoors).ToList();
                    var secondNotFirst = layout.OpenRoomDoors.Except(directions).ToList();
            
                    if (!firstNotSecond.Any() && !secondNotFirst.Any())
                        supportedRooms.Add(layout);
                }
                break;
            
            case RoomType.Boss:
                foreach (var layout in _bossRoomLayouts)
                {
                    var firstNotSecond = directions.Except(layout.OpenRoomDoors).ToList();
                    var secondNotFirst = layout.OpenRoomDoors.Except(directions).ToList();
            
                    if (!firstNotSecond.Any() && !secondNotFirst.Any())
                        supportedRooms.Add(layout);
                }
                break;
            
            case RoomType.Start:
                foreach (var layout in _startRoomLayouts)
                {
                    var firstNotSecond = directions.Except(layout.OpenRoomDoors).ToList();
                    var secondNotFirst = layout.OpenRoomDoors.Except(directions).ToList();
            
                    if (!firstNotSecond.Any() && !secondNotFirst.Any())
                        supportedRooms.Add(layout);
                }
                break;
            
            default:
                foreach (var layout in _roomLayouts)
                {
                    var firstNotSecond = directions.Except(layout.OpenRoomDoors).ToList();
                    var secondNotFirst = layout.OpenRoomDoors.Except(directions).ToList();
            
                    if (!firstNotSecond.Any() && !secondNotFirst.Any())
                        supportedRooms.Add(layout);
                }
                break;
        }
        
        return supportedRooms.GetRandomElement();
    }
    
    private T[,] Get2DArray<T>(T[] input, int height, int width)
    {
        var output = new T[height, width];
        
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                output[i, j] = input[i * width + j];
            }
        }
        return output;
    }

    private EntitySpawner InstantiateEntitySpawner(LevelEntity levelEntity, Vector2 origin)
    {
        var position = levelEntity.Position + origin;
        
        var spawner = Instantiate(_entitySpawner, position, quaternion.identity);
        spawner.EntityID = levelEntity.EntityID;

        return spawner;
    }
    
    private void MakeHoleInRoom(Direction direction, Vector3Int roomOrigin)
    {
        switch (direction)
        {
            case Direction.Top:
                _borderTilemap.SetTile(new Vector3Int(roomOrigin.x + 8, roomOrigin.y + 9, 0), null);
                _borderTilemap.SetTile(new Vector3Int(roomOrigin.x + 9, roomOrigin.y + 9, 0), null);
                break;
            case Direction.Bottom:
                _borderTilemap.SetTile(new Vector3Int(roomOrigin.x + 8, roomOrigin.y, 0), null);
                _borderTilemap.SetTile(new Vector3Int(roomOrigin.x + 9, roomOrigin.y, 0), null);
                break;
            case Direction.Left:
                _borderTilemap.SetTile(new Vector3Int(roomOrigin.x, roomOrigin.y + 4, 0), null);
                _borderTilemap.SetTile(new Vector3Int(roomOrigin.x, roomOrigin.y + 5, 0), null);
                break;
            case Direction.Right:
                _borderTilemap.SetTile(new Vector3Int(roomOrigin.x + 17, roomOrigin.y + 4, 0), null);
                _borderTilemap.SetTile(new Vector3Int(roomOrigin.x + 17, roomOrigin.y + 5, 0), null);
                break;
        }
    }

    private Door SpawnDoor(Direction direction, Vector3Int roomOrigin)
    {
        Door door = null;

        switch (direction)
        {
            case Direction.Top:
                door = Instantiate(_doorPrefab, new Vector2(roomOrigin.x + 9, roomOrigin.y + 9.5f), Quaternion.identity);
                door.DoorDestination = Vector2.up;
                break;
            case Direction.Bottom:
                door = Instantiate(_doorPrefab, new Vector2(roomOrigin.x + 9, roomOrigin.y + .5f), Quaternion.identity);
                door.DoorDestination = Vector2.down;
                break;
            case Direction.Left:
                door = Instantiate(_doorPrefab, new Vector2(roomOrigin.x + .5f, roomOrigin.y + 5), Quaternion.Euler(0, 0, 90));
                door.DoorDestination = Vector2.left;
                break;
            case Direction.Right:
                door = Instantiate(_doorPrefab, new Vector2(roomOrigin.x + 17.5f, roomOrigin.y + 5), Quaternion.Euler(0, 0, 90));
                door.DoorDestination = Vector2.right;
                break;
        }

        return door;
    }
}
