using UnityEngine;

public class CharacterAirState : CharacterState
{
    public CharacterAirState(Character character) : base(character)
    {
    }

    private bool IsDescending => _character.VerticalVelocity < 0;
    private bool IsGrounded => _character.IsGrounded;
    private bool JumpInputPressed => InputManager.GetJumpWasPressedThisFrame();
    private bool JumpInputReleased => InputManager.GetJumpWasReleasedThisFrame() || _character.JumpReleasedDuringBuffer;
    private bool CanJump => _character.CanJump();
    private bool DashInputPressed => InputManager.GetDashWasPressedThisFrame();
    private bool CanDash => _character.DashCooldown <= 0;

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (DashInputPressed && CanDash) _stateMachine.ChangeState(_character.DashState);
        else if (IsDescending && IsGrounded)
        {
            _character.Land();
            _stateMachine.ChangeState(_character.IdleState);
        }
        else if (JumpInputReleased && _character.IsJumping) _character.CancelJumpEarly();
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        _character.AirPhysics();
        _character.Move(InputManager.GetMovement(), _movementData.airHorizontalAcceleration, _movementData.airHorizontalDeceleration);
    }
}
