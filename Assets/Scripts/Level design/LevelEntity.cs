using UnityEngine;

[System.Serializable]
public class LevelEntity
{
    [SerializeField] private int _entityID;
    [SerializeField] private Vector2 _position;

    public int EntityID => _entityID;
    public Vector2 Position => _position;


    public LevelEntity(int entityID, Vector2 position)
    {
        _entityID = entityID;
        _position = position;
    }
}