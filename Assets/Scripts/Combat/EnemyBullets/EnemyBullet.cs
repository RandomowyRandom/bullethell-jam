using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float _lifespan = 10;
    
    protected Rigidbody2D Rigidbody2D;

    protected float Lifespan => _lifespan;
    
    private void Awake()
    {
        // cache references
        Rigidbody2D = GetComponent<Rigidbody2D>();
        
        // destroy
        Destroy(gameObject, Lifespan);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("PlayerBulletDestroy"))
            Destroy(gameObject);
    }

    public virtual void ShootBullet(Vector2 direction, float force)
    {
        Rigidbody2D.velocity = direction.normalized * force;
    }
}
