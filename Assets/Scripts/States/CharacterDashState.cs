using UnityEngine;

public class CharacterDashState : CharacterState
{
    public CharacterDashState(Character character) : base(character)
    {
    }

    private bool IsFinishedDashing => _character.DashTimer <= 0;
    
    public override void StateEnter()
    {
        base.StateEnter();
        _character.ResetDashTimers();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (IsFinishedDashing) _stateMachine.ChangeState(_character.IdleState);
    }
    
    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        _character.Dash();
    }
}
