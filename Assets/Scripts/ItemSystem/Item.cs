using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    [Header("Item stats")]
    [SerializeField] private Stats _stats;
    [Space] 
    [SerializeField] private Sprite _itemSprite;
    [SerializeField] private int _itemID;
    [SerializeField] private string _itemName;
    [SerializeField] [TextArea] private string _tooltip;


    public Sprite ItemSprite => _itemSprite;
    public int ItemID => _itemID;
    public string ItemName => _itemName;
    public Stats Stats => _stats;
    public string ItemTooltip => _tooltip;
}