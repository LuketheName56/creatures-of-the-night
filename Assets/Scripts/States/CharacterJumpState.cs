using UnityEngine;

public class CharacterJumpState : CharacterState
{
    private bool JumpInputPressed => InputManager.GetJumpWasPressedThisFrame() || _character.JumpBufferTimer > 0;
    private bool CanJumpParry => _character.CanJumpParry();

    public CharacterJumpState(Character character) : base(character)
    {
    }
    
    public override void StateEnter()
    {
        base.StateEnter();
        _character.Jump();
        _stateMachine.ChangeState(_character.AirState);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        // if (JumpInputPressed && CanJumpParry) _stateMachine.ChangeState(_character.JumpParryState);
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        _character.Move(InputManager.GetMovement(), _movementData.airHorizontalAcceleration, _movementData.airHorizontalDeceleration);
    }
}
