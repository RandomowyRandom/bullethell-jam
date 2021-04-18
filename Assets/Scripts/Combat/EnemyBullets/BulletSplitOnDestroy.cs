using UnityEngine;

public class BulletSplitOnDestroy : EnemyBullet
{
    [SerializeField] private int _numberOfProjectiles;
    [SerializeField] private float _force;
    [SerializeField] private EnemyBullet _bullet;
    
    private void OnDestroy()
    {
        var angleStep = 360f / _numberOfProjectiles;
        var angle = 0f;

        for (int i = 0; i <= _numberOfProjectiles - 1; i++)
        {
            var projectileDirXPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            var projectileDirYPosition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);
            
            var projVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
            var projMoveDir = (projVector - (Vector2) transform.position).normalized;
            
            var bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
            bullet.ShootBullet(projMoveDir, _force);

            angle += angleStep;
        }
    }
}