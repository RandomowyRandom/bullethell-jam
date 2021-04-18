using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float _lifespan;
    
    protected Rigidbody2D Rigidbody2D;

    protected float Lifespan => _lifespan;
    
    private void Awake()
    {
        // cache references
        Rigidbody2D = GetComponent<Rigidbody2D>();
        
        // destroy
        Destroy(gameObject, Lifespan);
    }

    public virtual void ShootBullet(Vector2 direction, float force)
    {
        Rigidbody2D.velocity = direction.normalized * force;
    }
}
