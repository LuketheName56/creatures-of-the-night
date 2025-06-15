using UnityEngine;

public class CharacterSwitchdashState : CharacterState
{
    public CharacterSwitchdashState(Character character) : base(character)
    {
    }

    private bool IsFinishedSwitchdashing => _character.SwitchdashTimer <= 0;

    public override void StateEnter()
    {
        base.StateEnter();
        // _character.Flip();
        _character.InitializeSwitchdashTimer();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        if (IsFinishedSwitchdashing)
        {
            _stateMachine.ChangeState(_character.IdleState);
        }
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        _character.Switchdash();
    }

    //can only be accessed in the Dash State
    //can only be activated within .5 seconds of the Dash State being activated
    //when in dash state, and H button pressed,
    //^change direction and increase the magnitude of velocity


    //if switchdashTimer > 0, the duration is continuing, continue in the Switchdash state. 
    //if switchdashTimer > 0, end switchdash. 

    //if switchdash COOLdown > 0, the dash just started and character may Enter the Switchdash state. 

}

