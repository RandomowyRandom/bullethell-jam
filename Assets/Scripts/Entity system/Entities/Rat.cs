using System.Collections;
using UnityEngine;

public class Rat : Entity
{
    public override int EntityID => 3;
    
    [SerializeField] private EnemyBullet _inPlayerBulletSplit;

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
            Instantiate(_inPlayerBulletSplit, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(2);
        }
    }
    
    private IEnumerator EntityAIMovement()
    {
        while (true)
        {
            var velocity = (PlayerVitalStats.Instance.transform.position - transform.position).normalized * (1.1f * Random.Range(-1, 2));
            _rigidbody2D.velocity = velocity;
            
            yield return new WaitForSeconds(.5f);
        }
    }
}