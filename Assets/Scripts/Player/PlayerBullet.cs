using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float _damage;
    public float Damage
    {
        get => _damage;
        set => _damage = value;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerBulletDestroy"))
        {
            Destroy(gameObject);
        }
    }
}