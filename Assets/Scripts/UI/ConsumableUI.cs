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
                
                _ketamineAmountUI.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                LeanTween.scale(_ketamineAmountUI.gameObject, Vector3.one, .4f);
                
                break;
            case PickupType.Vitamin:
                _vitaminAmountUI.SetText(inventory.VitaminAmount.ToString());
                
                
                _vitaminAmountUI.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                LeanTween.scale(_vitaminAmountUI.gameObject, Vector3.one, .4f);
                
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
