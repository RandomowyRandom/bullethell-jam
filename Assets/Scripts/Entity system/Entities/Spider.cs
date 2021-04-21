using System.Collections;
using UnityEngine;

public class Spider : Entity
{
    public override int EntityID => 2;
    
    [SerializeField] private EnemyBullet _inPlayerBullet;
    [SerializeField] private EnemyBullet _inPlayerBulletUp;
    [SerializeField] private EnemyBullet _inPlayerBulletDown;

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
                Instantiate(_inPlayerBulletUp, transform.position, Quaternion.identity);
                Instantiate(_inPlayerBulletDown, transform.position, Quaternion.identity);
    
                yield return new WaitForSeconds(.7f);
            }

            yield return new WaitForSeconds(1);
        }
    }
    
    private IEnumerator EntityAIMovement()
    {
        while (true)
        {
            var velocity = (PlayerVitalStats.Instance.transform.position - transform.position).normalized * 1.1f;
            _rigidbody2D.velocity = velocity;
            
            yield return new WaitForSeconds(1);
        }
    }
}