using System.Collections.Generic;
using UnityEngine;

public class EntityDatabase : MonoBehaviour
{
    [SerializeField] private List<Entity> _entities;
    private Dictionary<int, Entity> _database = new Dictionary<int, Entity>();

    private static EntityDatabase _instance;
    public static EntityDatabase Instance => _instance;

    private void Awake()
    {
        // TODO: better implementation of singleton
        // initialize singleton
        if (_instance == null)
            _instance = this;
        
        // initialize database
        foreach (var entity in _entities)
        {
            _database.Add(entity.EntityID, entity);
        }
    }

    public Entity GetEntity(int id)
    {
        var success = _database.TryGetValue(id, out var entity);
        if (success)
            return entity;
        
        Debug.LogError($"Entity of ID {id} is not implemented!");
        return null;
    }
}
