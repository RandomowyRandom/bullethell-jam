using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private int _lastDir = -1;
    private void Awake()
    {
        // cache references
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleRotationAnimation();
    }

    private void HandleAnimation()
    {
        if (_rigidbody2D.velocity.x > 0)
        {
            // go right
            _animator.Play("walkRight");
            _lastDir = 1;
        }
        else if (_rigidbody2D.velocity.x < 0)
        {
            // go left
            _animator.Play("walkLeft");
            _lastDir = -1;
        }
        else if (_rigidbody2D.velocity.y != 0)
        {
            switch (_lastDir)
            {
                case 1:
                    _animator.Play("walkRight");
                    break;
                case -1:
                    _animator.Play("walkLeft");
                    break;
                default:
                    _animator.Play("walkLeft");
                    break;
            }
        }
        else
        {
            switch (_lastDir)
            {
                case 1:
                    _animator.Play("idleRight");
                    break;
                case -1:
                    _animator.Play("idleLeft");
                    break;
                default:
                    _animator.Play("idleLeft");
                    break;
            }
        }
    }

    private void HandleRotationAnimation()
    {
        var mousePosition = Utilities.GetMousePosition();

        var aimDirection = (mousePosition - transform.position).normalized;
        var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        if (angle > 90 || angle < -90)
        {
            // left
            if (_rigidbody2D.velocity != Vector2.zero)
            {
                // player is moving
                _animator.Play("walkLeft");
            }
            else
            {
                _animator.Play("idleLeft");
            }
        }
        else
        {
            // right
            if (_rigidbody2D.velocity != Vector2.zero)
            {
                // player is moving 
                _animator.Play("walkRight");
            }
            else
            {
                _animator.Play("idleRight");
            }
        }
    }
}