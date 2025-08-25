using UnityEngine;

public class CharacterJumpState : CharacterState
{
    public CharacterJumpState(Character character) : base(character)
    {
    }
    
    public override void StateEnter()
    {
        base.StateEnter();
        _character.Jump();
        _stateMachine.ChangeState(_character.AirState);
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        _character.Move(InputManager.GetMovement(), _movementData.airHorizontalAcceleration, _movementData.airHorizontalDeceleration);
    }
}
