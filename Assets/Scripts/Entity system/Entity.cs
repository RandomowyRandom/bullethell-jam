using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private string _entityName;
    [SerializeField] private float _maxHealth;
    private float _health;

    public abstract int EntityID { get; }

    private void Awake()
    {
        // health init
        _health = _maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var bullet = other.gameObject.GetComponent<PlayerBullet>();

        if (bullet != null) 
        {
            TakeDamage(1); //TODO: replace with actual damage
        }
        
        OnCollision(other);
    }

    protected virtual void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            OnDeath();
            Destroy(gameObject);
        }
    }

    protected virtual void OnDeath()
    {
        
    }

    protected virtual void OnCollision(Collision2D other)
    {
        
    }
}
