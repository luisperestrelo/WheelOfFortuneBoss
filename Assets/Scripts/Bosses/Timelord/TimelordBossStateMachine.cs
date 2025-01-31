using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelordBossStateMachine
{
    public TimelordBossState currentState { get; private set; }

    public void Initialize(TimelordBossState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(TimelordBossState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
} 