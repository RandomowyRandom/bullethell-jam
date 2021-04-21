using System;
using BulletHell.Time;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerVitalStats))]
public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Transform _gunHandle;
    [SerializeField] private Transform _shootPosition;
    [SerializeField] private PlayerBullet _playerBullet;
    [SerializeField] private AudioClip _shootSound;
    private PlayerStats _playerStats;
    private PlayerVitalStats _playerVitalStats;
    private Timer _fireRate;

    private void Start()
    {
        // cache references
        _playerStats = GetComponent<PlayerStats>();
        _playerVitalStats = GetComponent<PlayerVitalStats>();
        
        // initialize timers
        _fireRate = new Timer(_playerStats.Stats.FireRate);
        
        // sub to events
        PlayerStats.OnPlayerStatsChanged += UpdateFireRateTimer;
    }

    private void Update()
    {
        HandleRotation();
        HandleShooing();
        
        // handle timers
        _fireRate.HandleTimerScaled();
    }

    private void HandleRotation()
    {
        var mousePosition = Utilities.GetMousePosition();

        var aimDirection = (mousePosition - transform.position).normalized;
        var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        _gunHandle.eulerAngles = new Vector3(0, 0, angle);

        var localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            // left
            localScale.y = -1;
        }
        else
        {
            // right
            localScale.y = 1;
        }

        _gunHandle.localScale = localScale;
    }

    private void HandleShooing()
    {
        if (Input.GetMouseButton(0) && _fireRate.IsDone())
        {
            var mousePosition = Utilities.GetMousePosition();
            var aimDirection = new Vector2(mousePosition.x - transform.position.x,
                mousePosition.y - transform.position.y).normalized;
            var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            
            var bullet = Instantiate(_playerBullet, _shootPosition.position, Quaternion.Euler(0, 0, angle));
            bullet.GetComponent<Rigidbody2D>().velocity = aimDirection * _playerStats.Stats.BulletSpeed;
            bullet.Damage = _playerStats.Stats.Damage * GetDrugDamage();
            Debug.Log(bullet.Damage);

            EZCameraShake.CameraShaker.Instance.ShakeOnce(1.2f, .4f, .2f, .1f);
            AudioSource.PlayClipAtPoint(_shootSound, Extensions.MainCamera.transform.parent.position);
            
            _fireRate.Reset();
        }
    }

    private void UpdateFireRateTimer(PlayerStats stats)
    {
        _fireRate.MaxCooldown = stats.Stats.FireRate;
    }

    private float GetDrugDamage()
    {
        var drug = _playerVitalStats.PlayerHealthState;

        switch (drug)
        {
            case PlayerHealthState.Normal:
                return 1;
            case PlayerHealthState.Ketamine:
                return 2;
            case PlayerHealthState.Vitamin:
                return .8f;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void OnDestroy()
    {
        PlayerStats.OnPlayerStatsChanged -= UpdateFireRateTimer;
    }
}
