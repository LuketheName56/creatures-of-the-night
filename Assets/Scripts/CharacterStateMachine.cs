using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine
{
    public CharacterState CurrentState { get; private set; }

    public void InitializeDefaultState(CharacterState startState)
    {
        CurrentState = startState;
        CurrentState.StateEnter();
    }

    public void ChangeState(CharacterState newState)
    {
        CurrentState.StateExit();
        CurrentState = newState;
        CurrentState.StateEnter();
    }
}