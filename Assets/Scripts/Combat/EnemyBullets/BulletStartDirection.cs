using System;
using UnityEngine;

public class BulletStartDirection : EnemyBullet
{
    [SerializeField] private Vector2 _startDirection;
    [SerializeField] private float _startSpeed;

    private void Start()
    {
        ShootBullet(_startDirection, _startSpeed);
    }
}