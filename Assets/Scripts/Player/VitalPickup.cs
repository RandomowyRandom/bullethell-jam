using UnityEngine;

public class VitalPickup : MonoBehaviour
{
    [SerializeField] private VitalStats _vitalEffects;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private AudioClip _soundEffect;

    private void Start()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, .5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerVitals = other.gameObject.GetComponent<PlayerVitalStats>();

        if (playerVitals != null)
        {
            playerVitals.Heal(_vitalEffects.Health);
            Instantiate(_particles, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_soundEffect, Extensions.MainCamera.transform.parent.position);
            Destroy(gameObject);
            // TODO: add support for ketamine and vitamin reduction
        }
    }
}