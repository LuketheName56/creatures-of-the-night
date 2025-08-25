using UnityEngine;

public class CharacterSwitchDashState : CharacterState
{
    public CharacterSwitchDashState(Character character) : base(character)
    {
    }

    private bool IsFinishedSwitchDashing => _character.SwitchDashTimer <= 0;
    private Vector2 _dashDirection;

    public override void StateEnter()
    {
        base.StateEnter();
        _character.InitializeSwitchDashTimer();
        _dashDirection = _character.IsFacingRight ? Vector2.left : Vector2.right;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (IsFinishedSwitchDashing)
        {
            _stateMachine.ChangeState(_character.IdleState);
        }
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        _character.Dash(_dashDirection, _movementData.switchdashDistance/_movementData.switchdashDuration);
    }

    //can only be accessed in the Dash State
    //can only be activated within .5 seconds of the Dash State being activated
    //when in dash state, and H button pressed,
    //^change direction and increase the magnitude of velocity


    //if switchdashTimer > 0, the duration is continuing, continue in the Switchdash state. 
    //if switchdashTimer > 0, end switchdash. 

    //if switchdash COOLdown > 0, the dash just started and character may Enter the Switchdash state. 

}
