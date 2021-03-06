using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private int _entityID;

    public int EntityID
    {
        get => _entityID;
        set => _entityID = value;
    }

    public void Spawn(out Entity entity)
    {
        entity = Instantiate(EntityDatabase.Instance.GetEntity(_entityID), transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
