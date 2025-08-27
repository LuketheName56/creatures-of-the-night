using System;
using UnityEngine;

public class CharacterIdleState : CharacterState
{
    public CharacterIdleState(Character character) : base(character)
    {
    }
    
    private bool IsFalling => !_character.IsGrounded;
    private bool IsMoving => Mathf.Abs(InputManager.GetMovement().x) > _movementData.moveThreshold;
    private bool JumpInputPressed => InputManager.GetJumpWasPressedThisFrame() || _character.JumpBufferTimer > 0;
    private bool CanJump => _character.CanJump();
    private bool CanJumpParry => _character.CanJumpParry();
    private bool DashInputPressed => InputManager.GetDashWasPressedThisFrame();
    private bool CanDash => _character.DashCooldown <= 0;
    
    public override void StateUpdate()
    {
        base.StateUpdate();
        if (DashInputPressed && CanDash) _stateMachine.ChangeState(_character.DashState);
        else if (IsFalling) _stateMachine.ChangeState(_character.AirState);
        else if (JumpInputPressed && CanJump) _stateMachine.ChangeState(_character.JumpState);
        else if (JumpInputPressed && CanJumpParry) _stateMachine.ChangeState(_character.JumpParryState);
        else if (IsMoving) _stateMachine.ChangeState(_character.WalkState);
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        _character.Move(InputManager.GetMovement(), _movementData.groundDeceleration, _movementData.groundDeceleration);
    }
}
