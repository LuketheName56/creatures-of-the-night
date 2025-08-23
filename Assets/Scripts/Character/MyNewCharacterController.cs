using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(Rigidbody2D))]
public class MyNewCharacterController : MonoBehaviour
{
    [SerializeField] private MyNewCharacterStats stats;
    private BoxCollider2D _col;
    private Rigidbody2D _rb;
    
    private bool _isGrounded;
    private bool _isJumping;
    private bool _isFalling;

    private float _verticalVelocity; // We want to make changes to our velocity in Update that we want to actually apply in FixedUpdate so we store the value in this variable
    
    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        InputChecks();
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        VerticalMovement();
        HorizontalMovement();
    }

    private void InputChecks()
    {
        if (MyNewInputManager.GetJumpWasPressedThisFrame() && _isGrounded)
        {
            _verticalVelocity = stats.InitialJumpVelocity;
        }
    }
    
    private void CollisionChecks()
    {
        _isGrounded = Physics2D.BoxCast(_col.bounds.center, _col.size, 0, Vector2.down, stats.collisionCheckDistance, stats.collisionLayerMask);
    }
    
    private void VerticalMovement()
    {
        if (!_isGrounded)
        {
            _verticalVelocity += stats.Gravity * Time.fixedDeltaTime;
        }
        
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _verticalVelocity);
    }
    
    private void HorizontalMovement()
    {
        _rb.linearVelocity = new Vector2(MyNewInputManager.GetMovement().x * stats.speed, _rb.linearVelocity.y);
    }
}