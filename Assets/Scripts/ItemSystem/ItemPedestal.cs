using TMPro;
using UnityEngine;

public class ItemPedestal : Entity
{
    [SerializeField] private SpriteRenderer _itemSpriteOnPedestal;
    [SerializeField] private TextMeshPro _itemName;
    [SerializeField] private TextMeshPro _itemTooltip;
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
        _itemName.SetText(item.name);
        _itemTooltip.SetText(item.ItemTooltip);

        _itemName.sortingOrder = 20;
        _itemTooltip.sortingOrder = 20;
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