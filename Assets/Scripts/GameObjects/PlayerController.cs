using System;
using System.Collections;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Inject] private InputService _inputService;

    [Header("Movement Settings")]
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private float movementSmoothing = 0.05f;
    [SerializeField] private Rigidbody2D _rigidbody;

    [Header("Debug")]
    [SerializeField] private bool _isGrounded;
    
    private Vector2 _movementSmoothVelocity;
    private bool _waitingForUngrounded;
    private int _groundLayer;
    private Coroutine _ungroundCoroutine;

    private void Start()
    {
        _inputService.PlayerActions.Jump.performed += OnJump;
        _groundLayer = LayerMask.NameToLayer("Ground");
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
        if (collision.gameObject.layer == _groundLayer)
        {
            _isGrounded = true;

            if (_ungroundCoroutine != null)
            {
                StopCoroutine(_ungroundCoroutine);
                _ungroundCoroutine = null;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayer)
        {
            if (_ungroundCoroutine == null)
            {
                _ungroundCoroutine = StartCoroutine(DelayedUnground());
            }
        }
    }

    private IEnumerator DelayedUnground()
    {
        _waitingForUngrounded = true;
        yield return new WaitForSeconds(0.1f);
    
        // Проверяем, что игрок не успел снова коснуться земли
        if (_waitingForUngrounded)
        {
            _isGrounded = false;
        }

        _ungroundCoroutine = null;
        _waitingForUngrounded = false;
    }

    private void MoveBall()
    {
        var moveInput = 0f;

        if (_inputService.PlayerActions.Left.ReadValue<float>() > 0.1f)
            moveInput = -1f;
        else if (_inputService.PlayerActions.Right.ReadValue<float>() > 0.1f)
            moveInput = 1f;

        if (Mathf.Abs(moveInput) > 0.1f && _isGrounded)
        {
            var targetVelocity = new Vector2(moveInput * maxSpeed, _rigidbody.linearVelocity.y);
            _rigidbody.linearVelocity = Vector2.SmoothDamp(_rigidbody.linearVelocity, targetVelocity, ref _movementSmoothVelocity, movementSmoothing);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
