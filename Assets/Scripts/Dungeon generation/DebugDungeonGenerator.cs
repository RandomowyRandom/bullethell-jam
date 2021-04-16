using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DebugDungeonGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _drawTilemap;
    [SerializeField] private TileBase _roomTile;
    [SerializeField] private TileBase _startRoomTile;
    [SerializeField] private TileBase _bossTile;
    [SerializeField] private TileBase _lootTile;
    [SerializeField] private GameObject _door;
    [SerializeField] private MapInfo _mapInfo;
    private DungeonMap _dungeonMap;
    
    private void Start()
    {
        _dungeonMap = new DungeonMap(_mapInfo);
        ProcessMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _drawTilemap.ClearAllTiles();
            
            _dungeonMap = new DungeonMap(_mapInfo);
            ProcessMap();
        }
    }

    private void ProcessMap()
    {
        // put down map
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                // foreach (var roomDirection in _dungeonMap.Map[i, j].RoomDirections)
                // {
                //     Instantiate(_door, new Vector2(i, j) + GetDoorPosition(roomDirection), Quaternion.identity);
                // }
                
                switch (_dungeonMap.Map[i, j].RoomType)
                {
                    case RoomType.None:
                        break;
                    case RoomType.Simple:
                        _drawTilemap.SetTile(new Vector3Int(i, j, 0), _roomTile);
                        break;
                    case RoomType.Start:
                        _drawTilemap.SetTile(new Vector3Int(i, j, 0), _startRoomTile);
                        break;
                    case RoomType.Loot:
                        _drawTilemap.SetTile(new Vector3Int(i, j, 0), _lootTile);
                        break;
                    case RoomType.Boss:
                        _drawTilemap.SetTile(new Vector3Int(i, j, 0), _bossTile);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private Vector2 GetDoorPosition(Direction direction)
    {
        switch (direction)
        {
            case Direction.Top:
                return new Vector2(.5f, 1f);
            case Direction.Bottom:
                return new Vector2(.5f, 0f);
            case Direction.Left:
                return new Vector2(0f, .5f);
            case Direction.Right:
                return new Vector2(1f, .5f);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}