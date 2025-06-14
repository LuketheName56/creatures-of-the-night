using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Character Movement")]
public class CharacterMovementData : ScriptableObject
{
    [Header("Grounded/Collisions Checks")]
    public LayerMask groundLayer;
    [Range(0.5f, 1f)] public float groundDetectionRayWidth = 1f;
    public float groundDetectionRayLength = 0.02f;
    [Range(0.5f, 1f)] public float ceilingDetectionRayWidth = 1f;
    public float ceilingDetectionRayLength = 0.02f;

    [Header("Walking")]
    [Range(0f, 1f)] public float moveThreshold = 0.25f;
    [Range(1f, 100f)] public float maxWalkSpeed = 12.5f;
    [Range(0.25f, 50f)] public float groundAcceleration = 5f;
    [Range(0.25f, 50f)] public float groundDeceleration = 20f;

    [Header("Dashing")]
    public float dashDuration = 0.5f;
    public float dashDistance = 1.5f;
    public float dashCooldown = 1f;
    
    [Header("Jumping and Falling")]
    public float jumpHeight = 6.5f;
    public float timeTillJumpApex = 0.35f;
    [Range(0.01f, 5f)] public float gravityOnReleaseMultiplier = 2f;
    [Range(0.5f, 1f)] public float apexThreshold = 0.97f;
    [Range(0.01f, 1f)] public float apexHangTime = 0.075f;
    
    [Header("Jump Buffer and Coyote Time")]
    [Range(0f, 1f)] public float jumpBufferTime = 0.125f;
    [Range(0f, 1f)] public float jumpCoyoteTime = 0.1f;
    
    [Header("Air Movement")]
    [Range(0.25f, 50f)] public float airHorizontalAcceleration = 5f;
    [Range(0.25f, 50f)] public float airHorizontalDeceleration = 20f;
    
    private const float JumpHeightCompensationFactor = 1.054f;
    public float Gravity { get; private set; }
    public float InitialJumpVelocity { get; private set; }
    private float AdjustedJumpHeight { get; set; }
    
    private void OnValidate() => CalculateJumpStats();
    private void OnEnable() => CalculateJumpStats();
    private void CalculateJumpStats()
    {
        AdjustedJumpHeight = jumpHeight * JumpHeightCompensationFactor;
        Gravity = -(2f * AdjustedJumpHeight) / Mathf.Pow(timeTillJumpApex, 2f);
        InitialJumpVelocity = Mathf.Abs(Gravity) * timeTillJumpApex;
    }
}    
