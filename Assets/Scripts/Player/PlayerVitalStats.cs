using System;
using BulletHell.Time;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShooter))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Collider2D))]
public class PlayerVitalStats : MonoBehaviour
{
    [FormerlySerializedAs("_startHealth")] [SerializeField] private float _maxHealth;
    [SerializeField] private Volume _postProcessing;
    [SerializeField] private AudioClip _hurtSound;
    [SerializeField] private AudioClip _drugUse;
    [Header("Volumes")]
    [SerializeField] private VolumeProfile _ketamineVolume;
    [SerializeField] private VolumeProfile _vitaminVolume;
    [SerializeField] private VolumeProfile _normalVolume;
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _playerMovement;
    private Collider2D _collider2D;
    private PlayerShooter _playerShooter;
    private PlayerStats _playerStats;
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
    private Timer _invincibilityFrames = new Timer(.5f);

    public float Health => _health;
    public float MaxHealth => _maxHealth;
    public PlayerHealthState PlayerHealthState => _playerHealthState;
    private bool IsDead => _health <= 0 || _vitaminInBlood >= 90 || _ketamineInBlood >= 90;
    
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
        _health = _maxHealth;
    }

    private void Start()
    {
        // cache references
        _playerInventory = GetComponent<PlayerInventory>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerMovement = GetComponent<PlayerMovement>();
        _collider2D = GetComponent<Collider2D>();
        _playerShooter = GetComponent<PlayerShooter>();
        _playerStats = GetComponent<PlayerStats>();
        
        // sub to events
        _ketamineTimer.OnDone += () => SetPlayerHealthState(PlayerHealthState.Normal);
        _vitaminTimer.OnDone += () => SetPlayerHealthState(PlayerHealthState.Normal);
    }

    private void Update()
    {
        // handle timers
        _ketamineTimer.HandleTimerUnscaled();
        _vitaminTimer.HandleTimerUnscaled();
        _invincibilityFrames.HandleTimerScaled();

        if (IsDead && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("Sewer_1");
        
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
        if(!_invincibilityFrames.IsDone()) return;
        
        _health -= damage;
        OnPlayerHealthChanged?.Invoke(this);
        _invincibilityFrames.Reset();
        
        EZCameraShake.CameraShaker.Instance.ShakeOnce(3.2f, 3.2f, .1f, .2f);
        AudioSource.PlayClipAtPoint(_hurtSound, Extensions.MainCamera.transform.parent.position);
        _spriteRenderer.color = Color.red;
        LeanTween.color(gameObject, Color.white, .3f);
        
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

                _ketamineInBlood += 15;
                _vitaminInBlood -= 15;
                Time.timeScale = Mathf.Clamp(1.5f - _playerStats.Stats.DrugEfficiency, 1.1f, 1.5f);
                AudioSource.PlayClipAtPoint(_drugUse, Extensions.MainCamera.transform.parent.position);
                
                _ketamineTimer.Reset();
                break;
            
            case PlayerHealthState.Vitamin:
                _postProcessing.profile = _vitaminVolume;
                
                _ketamineInBlood -= 15;
                _vitaminInBlood += 15;
                Time.timeScale = Mathf.Clamp(.6f - _playerStats.Stats.DrugEfficiency/2, .3f, .6f);
                AudioSource.PlayClipAtPoint(_drugUse, Extensions.MainCamera.transform.parent.position);
                
                _vitaminTimer.Reset();
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        
        if(_vitaminInBlood >= 90 || _ketamineInBlood >= 90)
            Death();
    }
    private void Death()
    {
        _spriteRenderer.enabled = false;
        _playerMovement.enabled = false;
        _collider2D.enabled = false;
        _playerShooter.enabled = false;
        Time.timeScale = 1f;
        _playerInventory.Items.Clear();
        
        GameObject.Find("DeadText").GetComponent<TextMeshProUGUI>().enabled = true;
    }

    private void OnApplicationQuit()
    {
        _playerInventory.Items.Clear();
    }
}