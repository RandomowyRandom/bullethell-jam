using System;
using UnityEngine;

public class ItemPedestal : Entity
{
    [SerializeField] private SpriteRenderer _itemSpriteOnPedestal;
    private Item _itemOnPedestal;
    
    private int _entityID = 0;
    public override int EntityID => _entityID;

    private void Start()
    {
        SetItem(ItemDatabase.Instance.GetRandomItem());
    }

    public void SetItem(Item item)
    {
        _itemOnPedestal = item;
        _itemSpriteOnPedestal.sprite = item.ItemSprite;
    }

    protected override void OnCollision(Collision2D other)
    {
        var playerInventory = other.gameObject.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.AddItem(_itemOnPedestal);
            
            OnDeath();
            Destroy(gameObject);
        }
    }

    protected override void TakeDamage(float damage)
    {
        
    }
}