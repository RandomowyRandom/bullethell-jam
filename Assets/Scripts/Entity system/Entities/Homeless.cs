using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Homeless : Entity
{
    public override int EntityID => 5;

    [SerializeField] private EnemyBullet _splitBulletUp;
    [SerializeField] private EnemyBullet _splitBulletDown;
    [SerializeField] private EnemyBullet _splitBulletLeft;
    [SerializeField] private EnemyBullet _splitBulletRight;
    [FormerlySerializedAs("_inPlayerVelocityOverTimeReverse")] [SerializeField] private EnemyBullet _inPlayerReverse;
    [SerializeField] private EnemyBullet _inPlayerVelocity;
    [SerializeField] private AudioClip _deathSound;
            
    private void Start()
    {
        StartCoroutine(EntityAIBullets());
    }
        
    private IEnumerator EntityAIBullets() 
    {
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(_splitBulletLeft, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(.2f);
                Instantiate(_splitBulletRight, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(.2f);
                Instantiate(_splitBulletUp, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(.2f);
                Instantiate(_splitBulletDown, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(.5f);
            }

            yield return new WaitForSeconds(1);
            
            for (int i = 0; i < 35; i++)
            {
                Instantiate(_inPlayerReverse, transform.position, Quaternion.identity);
                Instantiate(_inPlayerVelocity, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(.2f);
            }
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        AudioSource.PlayClipAtPoint(_deathSound, transform.position);
        GameObject.Find("BrekText").GetComponent<TextMeshProUGUI>().enabled = true;
    }
}