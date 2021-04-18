using UnityEngine;

public class BulletStartDirectionToPlayer : EnemyBullet
{
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _force;

    private void Start()
    {
        ShootBullet(
            (Vector2)PlayerVitalStats.Instance.transform.position - 
            (Vector2)transform.position + 
            _offset, _force);
    }
}