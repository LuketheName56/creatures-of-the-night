using UnityEngine;

public abstract class CharacterState
{
    protected Character _character;
    protected CharacterStateMachine _stateMachine;
    protected CharacterMovementData _movementData;
    protected bool _isExitingState;

    public CharacterState(Character character)
    {
        _character = character;
        _stateMachine = character.StateMachine;
        _movementData = character.MovementData;
    }
    
    public virtual void StateEnter()
    {
        _isExitingState = false;

        if (_character.ShowEnteredStateDebugLog)
            Debug.Log("Entered State: " + _stateMachine.CurrentState);
    }
    
    public virtual void StateExit()
    {
        _isExitingState = true;
    }

    public virtual void StateUpdate()
    {
        _character.TickTimers();
    }

    public virtual void StateFixedUpdate()
    {
        _character.CollisionChecks();
        _character.ApplyVelocity();
    }
}
