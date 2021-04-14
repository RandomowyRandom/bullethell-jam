using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelCreator : MonoBehaviour
{
    [Header("Room data")] 
    [SerializeField] private string _roomName;
    [SerializeField] private RoomData _roomToSave;
    [SerializeField] private List<Direction> _openRooms;

    [Header("Elements to save")] 
    [SerializeField] private Tilemap _levelTilemap;
    [SerializeField] private GameObject _entityParentObject;

    [ContextMenu("Save layout")]
    private void SaveRoom()
    {
        // save name and open doors
        _roomToSave.OpenRoomDoors = new List<Direction>(_openRooms);
        _roomToSave.RoomName = _roomName;
        
        // save tilemap data
        var tiles = new TileBase[16, 8];
        
        for (int x = 1; x <= 16; x++)
        {
            for (int y = 1; y <= 8; y++)
            {
                tiles[x - 1, y - 1] = _levelTilemap.GetTile(new Vector3Int(x, y, 0));
            }
        }

        _roomToSave.LevelTilemap = tiles;

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
}
