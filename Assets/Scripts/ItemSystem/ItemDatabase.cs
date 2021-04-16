using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<Item> _items;
    private Dictionary<int, Item> _database = new Dictionary<int, Item>();

    private Random _random = new Random();
    private static ItemDatabase _instance;
    public static ItemDatabase Instance => _instance;
    
    private void Awake()
    {
        // TODO: better implementation of singleton
        // initialize singleton
        if (_instance == null)
            _instance = this;
        
        // initialize database
        foreach (var item in _items)
        {
            _database.Add(item.ItemID, item);
        }
    }
    
    public Item GetItem(int id)
    {
        var success = _database.TryGetValue(id, out var item);
        if (success)
            return item;
        
        Debug.LogError($"Item of ID {id} is not implemented!");
        return null;
    }

    public Item GetRandomItem()
    {
        return _database.ElementAt(_random.Next(0, _database.Count)).Value;
    }
}
