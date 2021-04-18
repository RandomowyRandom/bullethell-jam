using System;
using TMPro;
using UnityEngine;

public class ConsumableUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ketamineAmountUI;
    [SerializeField] private TextMeshProUGUI _vitaminAmountUI;

    private void Start()
    {
        // sun to events
        PlayerInventory.OnConsumableAmountChanged += UpdateUI;
    }

    private void UpdateUI(PlayerInventory inventory, PickupType type)
    {
        switch (type)
        {
            case PickupType.Ketamine:
                _ketamineAmountUI.SetText(inventory.KetamineAmount.ToString());
                break;
            case PickupType.Vitamin:
                _vitaminAmountUI.SetText(inventory.VitaminAmount.ToString());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    
    private void OnDestroy()
    {
        PlayerInventory.OnConsumableAmountChanged -= UpdateUI;
    }
}
