using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraPhaseControllerState : MorsoraBossState
{
    private enum PhaseState
    {
        BasicAttack,
        SpecialAttack
    }

    private PhaseState currentPhaseState;

    public MorsoraPhaseControllerState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName) : base(stateMachine, bossController, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        currentPhaseState = PhaseState.BasicAttack;
        StartBasicAttackCycle();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void StartBasicAttackCycle()
    {
        currentPhaseState = PhaseState.BasicAttack;
        bossController.IncrementBasicAttackCounter();
        stateMachine.ChangeState(bossController.GetRandomBasicAttackState());
    }

    public void StartSpecialAttackCycle()
    {
        currentPhaseState = PhaseState.SpecialAttack;
        stateMachine.ChangeState(bossController.GetRandomSpecialAttackState());
    }

    public void SpecialAttackFinished()
    {
        // Called by special attack states when they finish
        StartBasicAttackCycle();
    }
}
