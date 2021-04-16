using System;
using UnityEngine;

public class PlayerVitalStats : MonoBehaviour
{
    public static event Action<PlayerVitalStats> OnPlayerHealthChanged;
    
    private float _health;

    public float Health => _health;

    public void TakeDamage(float damage) // TODO: later replace with DamageSource to make death cause
    {
        _health -= damage;
        OnPlayerHealthChanged.Invoke(this);
        
        if(_health <= 0 )
            Death();
    }

    public void Heal(float damage)
    {
        _health += damage;
        _health = Mathf.Clamp(_health, 0, 100);
        
        OnPlayerHealthChanged.Invoke(this);
    }
    private void Death()
    {
        
    }
}