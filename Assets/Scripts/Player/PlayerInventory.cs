using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static event Action<PlayerInventory> OnItemsInInventoryChanged;
    public static event Action<PlayerInventory, PickupType> OnConsumableAmountChanged;

    [SerializeField] private Inventory _inventory;
    private int _ketamineAmount;
    private int _vitaminAmount;

    public List<Item> Items => _inventory.Items;
    public int KetamineAmount => _ketamineAmount;
    public int VitaminAmount => _vitaminAmount;

    public void AddItem(Item item)
    {
        _inventory.Items.Add(item);
        OnItemsInInventoryChanged?.Invoke(this);
    }

    public void AddKetamine(int amount)
    {
        _ketamineAmount += amount;
        OnConsumableAmountChanged?.Invoke(this, PickupType.Ketamine);
    }

    public void AddVitamin(int amount)
    {
        _vitaminAmount += amount;
        OnConsumableAmountChanged?.Invoke(this, PickupType.Vitamin);
    }

    public void RemoveKetamine(int amount)
    {
        _ketamineAmount -= amount;
        OnConsumableAmountChanged?.Invoke(this, PickupType.Ketamine);
    }

    public void RemoveVitamin(int amount)
    {
        _vitaminAmount -= amount;
        OnConsumableAmountChanged?.Invoke(this, PickupType.Vitamin);
    }
}
