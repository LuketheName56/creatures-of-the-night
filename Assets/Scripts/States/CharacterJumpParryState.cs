public class CharacterJumpParryState : CharacterState
{
    public CharacterJumpParryState(Character character) : base(character)
    {
    }
    
    public override void StateEnter()
    {
        base.StateEnter();
        _character.JumpParry();
        _stateMachine.ChangeState(_character.AirState);
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        _character.Move(InputManager.GetMovement(), _movementData.airHorizontalAcceleration, _movementData.airHorizontalDeceleration);
    }
}
