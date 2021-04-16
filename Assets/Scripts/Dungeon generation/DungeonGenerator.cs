using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private List<RoomData> _roomLayouts;
    [SerializeField] private List<RoomData> _lootRoomLayouts;
    [SerializeField] private List<RoomData> _bossRoomLayouts;
    [SerializeField] private MapInfo _mapInfo;
    [SerializeField] private EntitySpawner _entitySpawner;
    [SerializeField] private Tilemap _drawTilemap;
    [SerializeField] private RoomData _border;
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
                    var room = GetRandomRoomAtDirection(cell.RoomDirections, dungeonMap.Map[i, j].RoomType);
                    SpawnRoom(GetRoomPosition(i, j), room);

                    // spawn borders
                    var borderPosition = GetBorderPosition(i, j);
                    SpawnRoomBorder(GetBorderPosition(i, j));
                    
                    // create doors
                    foreach (var cellRoomDirection in cell.RoomDirections)
                    {
                        MakeHoleInRoom(cellRoomDirection, borderPosition);
                        // TODO: instantiate some door prefab
                    }
                    
                    // instantiate entity spawners
                    foreach (var entity in room.Entities)
                    {
                        InstantiateEntitySpawner(entity, new Vector2(borderPosition.x, borderPosition.y));
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
                _drawTilemap.SetTile(new Vector3Int(x + origin.x, y + origin.y, 0), roomLayout[x, y]);
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
                    _drawTilemap.SetTile(new Vector3Int(x + origin.x, y + origin.y, 0), roomBorder[x, y]);
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

    private void InstantiateEntitySpawner(LevelEntity levelEntity, Vector2 origin)
    {
        var position = levelEntity.Position + origin;
        
        var spawner = Instantiate(_entitySpawner, position, quaternion.identity);
        spawner.EntityID = levelEntity.EntityID;
    }
    
    private void MakeHoleInRoom(Direction direction, Vector3Int roomOrigin)
    {
        switch (direction)
        {
            case Direction.Top:
                _drawTilemap.SetTile(new Vector3Int(roomOrigin.x + 8, roomOrigin.y + 9, 0), null);
                _drawTilemap.SetTile(new Vector3Int(roomOrigin.x + 9, roomOrigin.y + 9, 0), null);
                break;
            case Direction.Bottom:
                _drawTilemap.SetTile(new Vector3Int(roomOrigin.x + 8, roomOrigin.y, 0), null);
                _drawTilemap.SetTile(new Vector3Int(roomOrigin.x + 9, roomOrigin.y, 0), null);
                break;
            case Direction.Left:
                _drawTilemap.SetTile(new Vector3Int(roomOrigin.x, roomOrigin.y + 4, 0), null);
                _drawTilemap.SetTile(new Vector3Int(roomOrigin.x, roomOrigin.y + 5, 0), null);
                break;
            case Direction.Right:
                _drawTilemap.SetTile(new Vector3Int(roomOrigin.x + 17, roomOrigin.y + 4, 0), null);
                _drawTilemap.SetTile(new Vector3Int(roomOrigin.x + 17, roomOrigin.y + 5, 0), null);
                break;
        }
    }
}
