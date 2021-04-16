using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    private const float SPEED = 5f; // TODO: replace with stats.speed when implemented

    private Rigidbody2D _rigidbody2D;
    private PlayerStats _playerStats;
    private Vector2 _input;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void HandleMovement()
    {
        _rigidbody2D.velocity = _input.normalized * _playerStats.Stats.Speed;
    }
}
