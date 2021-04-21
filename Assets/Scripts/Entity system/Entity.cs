using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public static event Action<Entity> OnEntityDied;
    
    [SerializeField] private string _entityName;
    [SerializeField] private float _maxHealth;
    [SerializeField] private ParticleSystem _deathParticles;
    private float _health;

    public abstract int EntityID { get; }

    private void Awake()
    {
        // health init
        _health = _maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnCollision(other);
    }

    protected virtual void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            OnDeath();
            
            if(_deathParticles != null)
                Instantiate(_deathParticles, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }

    protected virtual void OnDeath()
    {
        OnEntityDied?.Invoke(this);
    }

    protected virtual void OnCollision(Collision2D other)
    {
        var bullet = other.gameObject.GetComponent<PlayerBullet>();

        if (bullet != null) 
        {
            TakeDamage(bullet.Damage);
            Destroy(other.gameObject);
        }
    }
}
