using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private int _entityID;

    public int EntityID => _entityID;
    
    public void Spawn()
    {
        //Instantiate(entitydb.getentity, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
