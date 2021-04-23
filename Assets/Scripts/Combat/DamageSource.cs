using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private bool _destroyOnCollision;
    
    private float _damage = 10;

    public float Damage
    {
        get => _damage;
        set => _damage = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerVitals = other.gameObject.GetComponent<PlayerVitalStats>();

        if (playerVitals != null)
        {
            playerVitals.TakeDamage(_damage);
            
            if(_destroyOnCollision)
                Destroy(gameObject);
        }
    }
}
