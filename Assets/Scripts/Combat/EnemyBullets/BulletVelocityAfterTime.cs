using UnityEngine;

public class BulletVelocityAfterTime : EnemyBullet
{
    [SerializeField] [Range(.95f, 1.05f)] float _factor;

    private void FixedUpdate()
    {
        Rigidbody2D.velocity *= _factor;
    }
}