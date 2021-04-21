using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PickupType _pickupType;
    [SerializeField] private AudioClip _soundEffect;
    
    private void Start()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, .5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
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
            
            AudioSource.PlayClipAtPoint(_soundEffect, Extensions.MainCamera.transform.parent.position);
            Destroy(gameObject);
        }
    }
}