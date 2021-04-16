using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelCreator : MonoBehaviour
{
    [Header("Room data")] 
    [SerializeField] private string _roomName;
    [SerializeField] private RoomType _roomType;
    [SerializeField] private RoomData _roomToSave;
    [SerializeField] private List<Direction> _openRooms;

    [Header("Elements to save")] 
    [SerializeField] private Tilemap _levelTilemap;
    [SerializeField] private GameObject _entityParentObject;

    [ContextMenu("Save layout")]
    private void SaveRoom()
    {
        // save name, open doors and room type
        _roomToSave.OpenRoomDoors = new List<Direction>(_openRooms);
        _roomToSave.RoomName = _roomName;
        _roomToSave.RoomType = _roomType;
        
        // save tilemap data
        var tiles = new TileBase[16, 8];
        
        for (int x = 1; x <= 16; x++)
        {
            for (int y = 1; y <= 8; y++)
            {
                tiles[x - 1, y - 1] = _levelTilemap.GetTile(new Vector3Int(x, y, 0));
            }
        }

        _roomToSave.LevelTilemap = Get1DArray(tiles);

        var entities = new List<LevelEntity>();
        
        // save entities
        for (int i = 0; i < _entityParentObject.transform.childCount; i++)
        {
            var child = _entityParentObject.transform.GetChild(i);

            var entity = new LevelEntity(child.GetComponent<EntitySpawner>().EntityID, child.transform.localPosition);
            entities.Add(entity);
        }

        _roomToSave.Entities = new List<LevelEntity>(entities);
    }
    private T[] Get1DArray<T>(T[,] input) => input.Cast<T>().ToArray();
}
