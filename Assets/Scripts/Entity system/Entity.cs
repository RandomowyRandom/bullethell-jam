using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private string _entityName;
    [SerializeField] private float _maxHealth;
    private float _health;

    public abstract int EntityID { get; }

    private void Start()
    {
        // health init
        _health = _maxHealth;
    }

    public void TakeDamage(float damage)
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
}
