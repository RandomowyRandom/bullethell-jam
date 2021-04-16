using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static event Action<PlayerInventory> OnItemsInInventoryChanged;
    public static event Action<PlayerInventory> OnKetamineAmountChanged;
    public static event Action<PlayerInventory> OnVitaminAmountChanged;
    
    private List<Item> _items = new List<Item>();
    private int _ketamineAmount;
    private int _vitaminAmount;

    public List<Item> Items => _items;
    public int KetamineAmount => _ketamineAmount;
    public int VitaminAmount => _vitaminAmount;

    public void AddItem(Item item)
    {
        _items.Add(item);
        OnItemsInInventoryChanged?.Invoke(this);
    }

    public void AddKetamine(int amount)
    {
        _ketamineAmount += amount;
        OnKetamineAmountChanged?.Invoke(this);
    }

    public void AddVitamin(int amount)
    {
        _vitaminAmount += amount;
        OnVitaminAmountChanged?.Invoke(this);
    }

    public void RemoveKetamine(int amount)
    {
        _ketamineAmount -= amount;
        OnKetamineAmountChanged?.Invoke(this);
    }

    public void RemoveVitamin(int amount)
    {
        _vitaminAmount -= amount;
        OnVitaminAmountChanged?.Invoke(this);
    }
}
