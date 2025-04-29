using System;
using System.Collections;
using AppCoreModule.Scripts.Extensions;
using Common;
using GameObjects;
using Services;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Inject] private InputService _inputService;

    [Header("Movement Settings")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float maxSpeed = 8f;

    [Header("Debug")]
    [SerializeField] private bool _isGrounded;
    
    private Coroutine _ungroundCoroutine;
    private Vector2 _movementSmoothVelocity;
    private bool _waitingForUngrounded;
    
    public Health Health { get; private set; }

    private void Start()
    {
        Health = new Health();
        Health.Init(3,3);
        _inputService.PlayerActions.Jump.performed += OnJump;
    }

    private void OnDestroy()
    {
        _inputService.PlayerActions.Jump.performed -= OnJump;
    }

    private void FixedUpdate()
    {
        MoveBall();
        LimitSpeed();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMasks.Ground)
        {
            _isGrounded = true;

            if (_ungroundCoroutine != null)
            {
                StopCoroutine(_ungroundCoroutine);
                _ungroundCoroutine = null;
            }
        }

        if (collision.gameObject.layer == LayerMasks.Damage)
        {
            Health.DealDamage(1);
        }

        if (collision.gameObject.layer == LayerMasks.Dead)
        {
            Health.DealDamage(Int32.MaxValue);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMasks.Ground)
        {
            _ungroundCoroutine ??= StartCoroutine(DelayedUnground());
        }
    }

    private IEnumerator DelayedUnground()
    {
        _waitingForUngrounded = true;
        yield return new WaitForSeconds(0.1f);
        
        if (_waitingForUngrounded)
        {
            _isGrounded = false;
        }

        _ungroundCoroutine = null;
        _waitingForUngrounded = false;
    }

    private void MoveBall()
    {
        var moveInput = _inputService.PlayerActions.Move.ReadValue<float>();

        if (Mathf.Abs(moveInput) > 0.1f)
        {
            var force = new Vector2(moveInput * moveForce, 0f);
            _rigidbody.AddForce(force);
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.SetNew(x: Mathf.Clamp(_rigidbody.linearVelocity.x, -maxSpeed, maxSpeed));
        }
    }

    private void LimitSpeed()
    {
        var horizontalSpeed = Mathf.Abs(_rigidbody.linearVelocity.x);

        if (horizontalSpeed > maxSpeed)
        {
            _rigidbody.linearVelocity = new Vector2(Mathf.Sign(_rigidbody.linearVelocity.x) * maxSpeed, _rigidbody.linearVelocity.y);
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!_isGrounded) return;

        _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
