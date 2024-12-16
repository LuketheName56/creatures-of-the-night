using UnityEngine;
using System.Collections;
using System;

namespace TarodevController
{
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// I have a premium version on Patreon, which has every feature you'd expect from a polished controller. Link: https://www.patreon.com/tarodev
    /// You can play and compete for best times here: https://tarodev.itch.io/extended-ultimate-2d-controller
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/tarodev
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;

        public bool doubleJumpLEVEL; //for ABILITY
        public float jumpsLeft;

        public bool enableControls = true; //for cutscenes

        private float _time;
        public bool activate = false;  // The variable to activate

        public bool activateSwitchDash = false;

        public bool canActivate = true;


        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public Vector2 LastFrameInput;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        IEnumerator ExampleCoroutine()
        {
            activate = true;
            Debug.Log("activated");
            //yield on a new YieldInstruction that waits for half a second.
            yield return new WaitForSeconds(0.5f);
            activate = false;
        }

        IEnumerator ActivationDuration()
        {
            activateSwitchDash = true;
            canActivate = false;
            Debug.Log("activateSwitchDash in progress");
            yield return new WaitForSeconds(0.25f);
            activateSwitchDash = false;
        }

        IEnumerator SwitchDashCooldown()
        {
            yield return new WaitForSeconds(0.15f);
            canActivate = false;
        }


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3)) //code for cutscenes
            {
                // Debug.Log("pressed 3");
                enableControls = !enableControls;
            }


            if (Input.GetKeyDown(KeyCode.E)) //code for switch-dash ability
            {                    
                Debug.Log("Pressed E");
                StartCoroutine(ExampleCoroutine());
            }

            if (activate)
            {
                if (Input.GetKeyDown(KeyCode.D) && activate)
                {
                    Debug.Log("Both pressed");
                    if (canActivate)
                    {
                        StartCoroutine(SwitchDashCooldown());
                        StartCoroutine(ActivationDuration());
                    }

                    // if (LastFrameInput.x != FrameInput.x)
                    // {
                    //     _frameVelocity.x *= 1.5f;
                    // }

                }
            }
        }


        private void FixedUpdate()
        {
            CheckCollisions();
        
            if (enableControls)
            {
                HandleJump();
                HandleDirection();
                HandleGravity();
                
                ApplyMovement();
                LastFrameInput = FrameInput;
            }
        }

        #region Collisions
        
        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;
        

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                jumpsLeft = 3;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                jumpsLeft -= 1;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }
            
            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump()
        {
            bool doubleJumpLEVEL = true; //for ABILITY

            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_grounded || CanUseCoyote || (doubleJumpLEVEL && jumpsLeft > 0)) ExecuteJump();

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            jumpsLeft -= 1;
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            //if you are not trying to move
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            //if you are trying to move 
            else
            {
                if (LastFrameInput.x != FrameInput.x)
                {
                    _frameVelocity.x *= -1;
                }
                
                if (activateSwitchDash)
                {
                    // _frameVelocity.x *= 1.25f;      
                    canActivate = false;
                }


                /*
                if (Input.GetKeyDown(KeyCode.D))
                {
                    Debug.Log("E pressed");
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (LastFrameInput.x != FrameInput.x)
                        {
                            _frameVelocity.x *= 1.5f;
                        }
                    }
                }
                */

                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void ApplyMovement() => _rb.velocity = _frameVelocity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}