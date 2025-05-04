using System;
using System.Collections;
using AppCoreModule.Scripts.Extensions;
using Common;
using Cysharp.Threading.Tasks;
using GameObjects;
using MyBox;
using Services;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Inject] private InputService _inputService;
    [Inject] private GameAudioService _audioService;

    [Header("Movement Settings")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float knockbackForceMultiplier = 0.33f;

    [Header("Debug")]
    [SerializeField, ReadOnly] private bool _isGrounded;
    [SerializeField, ReadOnly] private bool _enabled;

    public readonly ReactiveProperty<bool> IsKnockedBack = new();
    private Coroutine _ungroundCoroutine;
    private Transform _transform;
    private Vector2 _movementSmoothVelocity;
    private bool _waitingForUngrounded;

    public Action Finished;
    
    public Vector3 Position => _transform.position;
    public bool IsEnabled => !Health.IsDead.Value && _enabled;

    public Health Health { get; private set; } = new();

    public void TakeDamage(Vector3 contactPoint, int damage)
    {
        var knockBackDirection = (_transform.position - contactPoint).normalized;
        _audioService.PlaySfx("damage");
        ApplyKnockBack(knockBackDirection, jumpForce * knockbackForceMultiplier);
        Health.DealDamage(damage);
    }
    
    public void ApplyKnockBack(Vector2 direction, float knockbackForce)
    {
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        IsKnockedBack.Value = true;
        ResetKnockbackState(0.5f).Forget();
    }
    
    private void Awake()
    {
        _transform = transform;
        _enabled = true;
    }

    private void Start()
    {
        Health.Init(3,3);
        _inputService.PlayerActions.Jump.performed += OnJump;
    }

    private void OnDestroy()
    {
        _inputService.PlayerActions.Jump.performed -= OnJump;
    }

    private void FixedUpdate()
    {
        if (!IsEnabled || IsKnockedBack.Value) return;
        
        MoveBall();
        LimitSpeed();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsEnabled) return;
        
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
            TakeDamage(collision.transform.position, 1);
        }

        if (collision.gameObject.layer == LayerMasks.Dead)
        {
            Health.DealDamage(Int32.MaxValue);
            _audioService.PlaySfx("damage");
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!IsEnabled) return;
        
        if (col.gameObject.layer == LayerMasks.Dead)
        {
            Health.DealDamage(Int32.MaxValue);
            _audioService.PlaySfx("damage");
        }
        
        if (col.gameObject.CompareTag("Finish"))
        {
            _enabled = false;
            Finished?.Invoke();
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
        if (!IsEnabled || !_isGrounded || IsKnockedBack.Value) return;

        _audioService.PlaySfx("jump");
        _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    
    private async UniTask ResetKnockbackState(float delayInSeconds)
    {
        await UniTask.WaitForSeconds(delayInSeconds);
        IsKnockedBack.Value = false;
    }
}