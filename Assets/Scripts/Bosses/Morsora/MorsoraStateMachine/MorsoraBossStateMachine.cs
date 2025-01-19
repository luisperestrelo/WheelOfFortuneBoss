using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraBossStateMachine
{

    public MorsoraBossState currentState { get; private set; }



    public void Initialize(MorsoraBossState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(MorsoraBossState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }




}
