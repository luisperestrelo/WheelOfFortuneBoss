using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelordIdleState : TimelordBossState
{
    public TimelordIdleState(TimelordBossStateMachine stateMachine, TimelordBossController bossController, string animBoolName)
        : base(stateMachine, bossController, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        bossController.IncrementBasicAttackCounter();

        Debug.Log("basic attack counter is " + bossController.basicAttackCounter);

        if (bossController.shouldTransitionToPhase2)
        {
            //stateMachine.ChangeState(bossController.tentacleShieldState);
            return;
        }

        if (bossController.basicAttackCounter == 0)
        {
            stateMachine.ChangeState(bossController.GetRandomSpecialAttackState());
        }
        else
        {
            stateMachine.ChangeState(bossController.GetRandomBasicAttackState());
        }
    }

    public override void Update()
    {
        base.Update();

        if (timer > 5f)
        {
            // Example fallback in case we stay in Idle too long:
            // TimelordBossState randomState = bossController.GetRandomState();
            // stateMachine.ChangeState(randomState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
} 