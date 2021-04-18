using System;
using BulletHell.Time;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerInventory))]
public class PlayerVitalStats : MonoBehaviour
{
    [SerializeField] private float _startHealth;
    [SerializeField] private Volume _postProcessing;
    [Header("Volumes")]
    [SerializeField] private VolumeProfile _ketamineVolume;
    [SerializeField] private VolumeProfile _vitaminVolume;
    [SerializeField] private VolumeProfile _normalVolume;
    public static PlayerVitalStats Instance { get; private set; }
    public static event Action<PlayerVitalStats> OnPlayerHealthChanged;
    public static event Action<PlayerVitalStats, PlayerHealthState> OnPlayerHealthStateChanged;
    
    private float _health;
    private float _ketamineInBlood = 50;
    private float _vitaminInBlood = 50;
    private PlayerHealthState _playerHealthState;
    private PlayerInventory _playerInventory;
    private Timer _ketamineTimer = new Timer(10);
    private Timer _vitaminTimer = new Timer(10);

    public float Health => _health;
    public float KetamineInBlood
    {
        get => _ketamineInBlood;
        set => _ketamineInBlood = value;
    }
    public float VitaminInBlood
    {
        get => _vitaminInBlood;
        set => _vitaminInBlood = value;
    }

    private void Awake()
    {
        // singleton
        if (Instance == null)
            Instance = this;
        // init
        _health = _startHealth;
    }

    private void Start()
    {
        // cache references
        _playerInventory = GetComponent<PlayerInventory>();
        
        // sub to events
        _ketamineTimer.OnDone += () => SetPlayerHealthState(PlayerHealthState.Normal);
        _vitaminTimer.OnDone += () => SetPlayerHealthState(PlayerHealthState.Normal);
    }

    private void Update()
    {
        // handle timers
        _ketamineTimer.HandleTimer();
        _vitaminTimer.HandleTimer();
        
        // read input for using Ketamine and Vitamin
        if(_playerHealthState != PlayerHealthState.Normal) return;
        
        if (Input.GetKeyDown(KeyCode.E) && _playerInventory.KetamineAmount > 0)
        {
            SetPlayerHealthState(PlayerHealthState.Ketamine);
            _playerInventory.RemoveKetamine(1);
        }
        else if(Input.GetKeyDown(KeyCode.Q) && _playerInventory.VitaminAmount > 0)
        {
            SetPlayerHealthState(PlayerHealthState.Vitamin);
            _playerInventory.RemoveVitamin(1);
        }
    }

    public void TakeDamage(float damage) // TODO: later replace with DamageSource to make death cause
    {
        _health -= damage;
        OnPlayerHealthChanged?.Invoke(this);
        
        if(_health <= 0 )
            Death();
    }

    public void Heal(float damage)
    {
        _health += damage;
        _health = Mathf.Clamp(_health, 0, 100);
        
        OnPlayerHealthChanged?.Invoke(this);
    }

    public void SetPlayerHealthState(PlayerHealthState state)
    {
        _playerHealthState = state;
        HandleHealthStateChanged(state);
        OnPlayerHealthStateChanged?.Invoke(this, state);
    }
    
    private void HandleHealthStateChanged(PlayerHealthState state)
    {
        switch (state)
        {
            case PlayerHealthState.Normal:
                Time.timeScale = 1f;
                _postProcessing.profile = _normalVolume;
                break;
            case PlayerHealthState.Ketamine:
                _postProcessing.profile = _ketamineVolume;

                _ketamineInBlood += 10;
                _vitaminInBlood -= 10;
                Time.timeScale = 1.5f;
                
                _ketamineTimer.Reset();
                break;
            case PlayerHealthState.Vitamin:
                _postProcessing.profile = _vitaminVolume;
                
                _ketamineInBlood -= 10;
                _vitaminInBlood += 10;
                Time.timeScale = .5f;
                
                _vitaminTimer.Reset();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
    private void Death()
    {
        
    }
}