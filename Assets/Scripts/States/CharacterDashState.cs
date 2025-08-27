using UnityEngine;

public class CharacterDashState : CharacterState
{
    public CharacterDashState(Character character) : base(character)
    {
    }
    private bool IsFinishedDashing => _character.DashTimer <= 0;
    private bool SwitchDashInputPressed => InputManager.GetSwitchdashWasPressedThisFrame();
    private bool CanSwitchDash => _character.SwitchDashCooldown >= 0;
    private Vector2 _dashDirection;
    private bool JumpInputPressed => InputManager.GetJumpWasPressedThisFrame();
    private bool CanJumpParry => _character.CanJumpParry();

    
    public override void StateEnter()
    {
        base.StateEnter();
        _character.InitializeDashTimers();
        _dashDirection = _character.IsFacingRight ? Vector2.right : Vector2.left;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (JumpInputPressed && CanJumpParry) _stateMachine.ChangeState(_character.JumpParryState);
        if (SwitchDashInputPressed && CanSwitchDash)
        {
            _stateMachine.ChangeState(_character.SwitchDashState);
        }
        else if (IsFinishedDashing)
        {
            _stateMachine.ChangeState(_character.IdleState);
        }
    }
    
    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        _character.Dash(_dashDirection, _movementData.dashDistance/_movementData.dashDuration);
    }
}
