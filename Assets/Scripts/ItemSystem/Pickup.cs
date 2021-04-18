using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PickupType _pickupType;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var playerInventory = other.gameObject.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            switch (_pickupType)
            {
                case PickupType.Ketamine:
                    playerInventory.AddKetamine(1);
                    break;
                case PickupType.Vitamin:
                    playerInventory.AddVitamin(1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Destroy(gameObject);
        }
    }
}