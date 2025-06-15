using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Properties & References
    
    [Header("References")]
    public CharacterMovementData MovementData;
    public Rigidbody2D rb;
    public Collider2D col;
    
    [Header("State Machine")]
    public CharacterStateMachine StateMachine;
    public CharacterIdleState IdleState;
    public CharacterWalkState WalkState;
    public CharacterAirState AirState;
    public CharacterJumpState JumpState;
    public CharacterDashState DashState;
    public CharacterSwitchdashState SwitchdashState;

    [Header("Debug")]
    public bool ShowEnteredStateDebugLog;
    public bool ShowGroundedCollisionBox;
    public bool ShowCeilingCollisionBox;

    #endregion
    
    private void Awake()
    {
        StateMachine = new CharacterStateMachine();
        IdleState = new CharacterIdleState(this);
        WalkState = new CharacterWalkState(this);
        AirState = new CharacterAirState(this);
        JumpState = new CharacterJumpState(this);
        DashState = new CharacterDashState(this);
        SwitchdashState = new CharacterSwitchdashState(this);
    }
    
    private void Start()
    {
        StateMachine.InitializeDefaultState(IdleState);
    }
    
    private void Update()
    {
        StateMachine.CurrentState.StateUpdate();
    }
    
    private void FixedUpdate()
    {
        StateMachine.CurrentState.StateFixedUpdate();
    }
    
    #region Physics
    
    public float HorizontalVelocity { get; private set; }
    public float VerticalVelocity { get; private set; }
    
    public void ApplyVelocity()
    {
        rb.linearVelocity = new Vector2(HorizontalVelocity, VerticalVelocity);
    }

    public void SetVerticalVelocity(float velocity) => VerticalVelocity = velocity;
    public void IncrementVerticalVelocity(float increment) => VerticalVelocity += increment;

    #endregion
    
    #region Timers and Cooldowns

    public void TickTimers()
    {
        TickJumpBufferTimer();
        TickCoyoteTimeTimer();
        TickJumpApexTimer();
        TickDashTimers();
        TickSwitchdashTimer();
    }
    
    #endregion
    
    #region Collision Detection
    
    private RaycastHit2D _groundHit;
    public bool IsGrounded { get; private set; }
    private RaycastHit2D _ceilingHit;
    public bool IsTouchingCeiling { get; private set; }
    
    public void CollisionChecks()
    {
        CheckForGrounded();
        CheckForCeiling();
    }

    public void CheckForGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(col.bounds.center.x, col.bounds.min.y);
        Vector2 boxCastSize = new Vector2(col.bounds.size.x * MovementData.groundDetectionRayWidth, MovementData.groundDetectionRayLength);
        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MovementData.groundDetectionRayLength, MovementData.groundLayer);
        IsGrounded = _groundHit.collider;
        
        #region Debug Visualization
        if (ShowGroundedCollisionBox)
        {
            Color rayColor = IsGrounded ? Color.green : !IsJumping && !IsFastFalling && CoyoteTimer > 0 ? Color.yellow : Color.red;
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MovementData.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MovementData.groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MovementData.groundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion
    }

    public void CheckForCeiling()
    {
        Vector2 boxCastOrigin = new Vector2(col.bounds.center.x, col.bounds.max.y);
        Vector2 boxCastSize = new Vector2(col.bounds.size.x * MovementData.ceilingDetectionRayWidth, MovementData.ceilingDetectionRayLength);
        _ceilingHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MovementData.ceilingDetectionRayLength, MovementData.groundLayer);
        IsTouchingCeiling = _ceilingHit.collider;
        
        #region Debug Visualization
        if (ShowCeilingCollisionBox)
        {
            Color rayColor = IsTouchingCeiling ? Color.red : Color.green;
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.up * MovementData.ceilingDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.up * MovementData.ceilingDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y + MovementData.ceilingDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion
    }
    
    #endregion
    
    #region Walking and Dashing

    public bool IsFacingRight { get; private set; } = true;
    
    #region Timers

    public float DashTimer { get; private set; }
    public float DashCooldown { get; private set; }

    //amount of time that can pass after Dash is activated before Switchdash can no longer be activated
    public float SwitchdashCooldown { get; private set; }

    //amount of time Switchdash lasts (after regular dash)
    public float SwitchdashTimer { get; private set; }

    
    public void InitializeDashTimers()
    {
        DashTimer = MovementData.dashDuration;
        DashCooldown = MovementData.dashCooldown;
        //switchdash cooldown depends on how long it's been since dash started
        SwitchdashCooldown = MovementData.switchdashCooldown;
    }
    
    public void InitializeSwitchdashTimer()
    {
        SwitchdashTimer = MovementData.switchdashDuration;
    }

    private void TickDashTimers()
    {
        if (StateMachine.CurrentState == DashState)
        {
            DashTimer -= Time.deltaTime;
            SwitchdashCooldown -= Time.deltaTime;
        }
        else
            DashCooldown -= Time.deltaTime;

        DashTimer = Mathf.Clamp(DashTimer, 0, MovementData.dashDuration);
        DashCooldown = Mathf.Clamp(DashCooldown, 0, MovementData.dashCooldown);
        SwitchdashCooldown = Mathf.Clamp(SwitchdashTimer, 0, MovementData.dashCooldown);
    }
    
    private void TickSwitchdashTimer()
    {
        if (StateMachine.CurrentState == SwitchdashState)
            SwitchdashTimer -= Time.deltaTime;
        
        SwitchdashTimer = Mathf.Clamp(SwitchdashTimer, 0, MovementData.switchdashDuration);
    }

    #endregion
    
    #region Functions
    
    public void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        bool isMoving = Mathf.Abs(moveInput.x) > MovementData.moveThreshold;
        if (isMoving)
        {
            CheckForFlip(moveInput);
            float targetVelocity = moveInput.x * MovementData.maxWalkSpeed;
            HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, targetVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, 0f, deceleration * Time.deltaTime);
        }
    }
    
    public void Dash()
    {
        Vector2 direction = IsFacingRight ? Vector2.right : Vector2.left;
        HorizontalVelocity = direction.x * (MovementData.dashDistance / MovementData.dashDuration);
        SetVerticalVelocity(0);
    }
    
    public void Switchdash()
    {
        Vector2 newDirection = IsFacingRight ? Vector2.left : Vector2.right;
        //changes direction and increases velocity since switchdashDistance > dashDistance
        HorizontalVelocity = newDirection.x * (MovementData.switchdashDistance / MovementData.dashDuration);
        Vector2 flipInput = new Vector2(HorizontalVelocity, 0);
        // CheckForFlip(flipInput);
        SetVerticalVelocity(0);   
    }


    private void CheckForFlip(Vector2 moveInput)
    {
        bool movingRight = moveInput.x > 0;
        if (IsFacingRight != movingRight)
        {
            IsFacingRight = movingRight;
            transform.Rotate(0f, movingRight ? 180f : -180f, 0f);
        }
    }
    
    #endregion
    
    #endregion
    
    #region Jumping and Falling

    public bool IsJumping { get; private set; }
    public bool IsFastFalling { get; private set; }
    public float JumpBufferTimer { get; set; }
    public bool JumpReleasedDuringBuffer { get; set; }
    public float CoyoteTimer { get; private set; }
    public float JumpApexPoint { get; private set; }
    public float JumpApexTimer { get; private set; }
    public bool JumpApexTimerStarted { get; set; }
    public float FallTimer { get; private set; }
    
    #region Checks
    
    public bool CanJump() => !IsJumping && !IsFastFalling && (IsGrounded || CoyoteTimer > 0f);

    #endregion

    #region Timers

    private void TickJumpBufferTimer()
    {
        if (InputManager.GetJumpWasPressedThisFrame())
        {
            JumpBufferTimer = MovementData.jumpBufferTime;
            JumpReleasedDuringBuffer = false;
        }
        else
        {
            JumpBufferTimer -= Time.deltaTime;
        }

        if (InputManager.GetJumpWasReleasedThisFrame() && JumpBufferTimer > 0)
            JumpReleasedDuringBuffer = true;
        
        JumpBufferTimer = Mathf.Clamp(JumpBufferTimer, 0, MovementData.jumpBufferTime);
    }

    private void TickCoyoteTimeTimer()
    {
        if (IsGrounded)
            CoyoteTimer = MovementData.jumpCoyoteTime;
        else
            CoyoteTimer -= Time.deltaTime;
        
        CoyoteTimer = Mathf.Clamp(CoyoteTimer, 0, MovementData.jumpCoyoteTime);
    }

    private void TickJumpApexTimer()
    {
        if (!IsJumping)
        {
            JumpApexTimer = 0;
            JumpApexTimerStarted = false;
        }
        else
        {
            JumpApexPoint = Mathf.InverseLerp(MovementData.InitialJumpVelocity, 0f, Mathf.Abs(VerticalVelocity));
            if (!JumpApexTimerStarted && JumpApexPoint > MovementData.apexThreshold)
            {
                JumpApexTimer = MovementData.apexHangTime;
                JumpApexTimerStarted = true;
            }
            else
                JumpApexTimer -= Time.deltaTime;
        }
        
        JumpApexTimer = Mathf.Clamp(JumpApexTimer, 0, MovementData.apexHangTime);
    }
    
    #endregion
    
    #region Functions

    public void Jump()
    {
        SetVerticalVelocity(MovementData.InitialJumpVelocity);
        IsJumping = true;
        IsFastFalling = false;
        JumpBufferTimer = 0;
    }

    public void CancelJumpEarly()
    {
        IsJumping = false;
        IsFastFalling = true;
        JumpReleasedDuringBuffer = false;
    }
    
    public void Land()
    {
        SetVerticalVelocity(0);
        IsJumping = false;
        IsFastFalling = false;
    }
    
    public void AirPhysics()
    {
        if (IsJumping)
        {
            if (IsTouchingCeiling)
            {
                SetVerticalVelocity(0);
                IsJumping = false;
                IsFastFalling = true;
            }
            else if (JumpApexTimerStarted)
            {
                if (JumpApexTimer > 0)
                {
                    SetVerticalVelocity(0f);
                }
                else
                {
                    IsJumping = false;
                    IsFastFalling = true;
                }
            }
            else
            {
                IncrementVerticalVelocity(MovementData.Gravity * Time.fixedDeltaTime);
            }
        }

        if (IsFastFalling)
        {
            IncrementVerticalVelocity(MovementData.Gravity * MovementData.gravityOnReleaseMultiplier * Time.fixedDeltaTime);
        }
        else
            IncrementVerticalVelocity(MovementData.Gravity * Time.fixedDeltaTime);
    }
    
    #endregion
    
    #endregion
}
