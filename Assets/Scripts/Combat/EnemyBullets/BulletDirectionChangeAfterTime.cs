using System.Collections;
using UnityEngine;

public class BulletDirectionChangeAfterTime : EnemyBullet
{
    [SerializeField] private bool _reverse;
    [SerializeField] private float _time;
    [SerializeField] private Vector2 _direction;

    private void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    private IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(_time);
        
        if (_reverse)
            Rigidbody2D.velocity = -Rigidbody2D.velocity;
        else
        {
            var magnitude = Rigidbody2D.velocity.magnitude;
            Rigidbody2D.velocity = _direction.normalized * magnitude;
        }
    }
}