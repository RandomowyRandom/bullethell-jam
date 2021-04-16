using UnityEngine;

public class VitalPickup : MonoBehaviour
{
    [SerializeField] private VitalStats _vitalEffects;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var playerVitals = other.gameObject.GetComponent<PlayerVitalStats>();

        if (playerVitals != null)
        {
            playerVitals.Heal(_vitalEffects.Health);
            // TODO: add support for ketamine and vitamin reduction
        }
    }
}