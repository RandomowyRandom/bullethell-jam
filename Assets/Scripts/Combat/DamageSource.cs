using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private bool _destroyOnCollision;
    
    private float _damage;

    public float Damage
    {
        get => _damage;
        set => _damage = value;
    }

    private void OnCollisionEnter2D(Collision2D other)
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
