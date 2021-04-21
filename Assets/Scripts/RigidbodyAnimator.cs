using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class RigidbodyAnimator : MonoBehaviour
{
    [SerializeField] private string _walkLeft = "walkLeft";
    [SerializeField] private string _walkRight = "walkRight";
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private void Awake()
    {
        // cache references
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleAnimator();
    }

    private void HandleAnimator()
    {
        if (_rigidbody2D.velocity.x > 0)
        {
            // go right
            _animator.Play(_walkRight);
        }
        else if (_rigidbody2D.velocity.x < 0)
        {
            // go left
            _animator.Play(_walkLeft);
        }
    }
}
