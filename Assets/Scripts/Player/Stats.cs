using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _range;
    [SerializeField] private float _drugEfficiency;
    [SerializeField] private float _bulletSpeed;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public float Damage
    {
        get => _damage;
        set => _damage = value;
    }

    public float FireRate
    {
        get => _fireRate;
        set => _fireRate = value;
    }

    public float Range
    {
        get => _range;
        set => _range = value;
    }

    public float DrugEfficiency
    {
        get => _drugEfficiency;
        set => _drugEfficiency = value;
    }

    public float BulletSpeed
    {
        get => _bulletSpeed;
        set => _bulletSpeed = value;
    }
}