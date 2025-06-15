using UnityEngine;

public class CharacterDashState : CharacterState
{
    public CharacterDashState(Character character) : base(character)
    {
    }

    private bool IsFinishedDashing => _character.DashTimer <= 0;
    private bool SwitchdashInputPressed => InputManager.GetSwitchdashWasPressedThisFrame();
    private bool CanSwitchdash => _character.SwitchdashCooldown >= 0;
 
    public override void StateEnter()
    {
        base.StateEnter();
        _character.InitializeDashTimers();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (SwitchdashInputPressed && CanSwitchdash)
        {
            _stateMachine.ChangeState(_character.SwitchdashState);
        }
        else if (IsFinishedDashing)
        {
            _stateMachine.ChangeState(_character.IdleState);
        }
    }
    
    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        _character.Dash();
    }
}
