using System.Collections;
using UnityEngine;

public class Crocodile : Entity
{
    public override int EntityID => 1;
    
    [SerializeField] private EnemyBullet _inPlayerBullet;
    [SerializeField] private EnemyBullet _spreadBullet;
    private Rigidbody2D _rigidbody2D;
    
    private void Start()
    {
        // get references
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        StartCoroutine(EntityAIBullets());
        StartCoroutine(EntityAIMovement());
    }

    private IEnumerator EntityAIBullets()
    {
        while (true)
        {
            for (var i = 0; i < 5; i++)
            {
                Instantiate(_inPlayerBullet, transform.position, Quaternion.identity);

                yield return new WaitForSeconds(.5f);
            }

            Instantiate(_spreadBullet, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator EntityAIMovement()
    {
        while (true)
        {
            var randomDir = (Vector2)UnityEngine.Random.onUnitSphere.normalized;
            _rigidbody2D.velocity = randomDir * 1.2f;

            yield return new WaitForSeconds(2);
        }
    }
}
