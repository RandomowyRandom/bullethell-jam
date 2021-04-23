using System.Collections;
using UnityEngine;

public class VitalPickup : MonoBehaviour
{
    [SerializeField] private VitalStats _vitalEffects;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private AudioClip _soundEffect;
    private Collider2D _collider2D;

    private void Start()
    {
        // cache references
        _collider2D = GetComponent<Collider2D>();
        
        _collider2D.enabled = false;
        StartCoroutine(EnableCollider());
        
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
    
    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(1.1f);
        _collider2D.enabled = true;
    }
}