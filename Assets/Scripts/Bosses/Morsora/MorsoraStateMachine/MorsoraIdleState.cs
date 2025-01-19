using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraIdleState : MorsoraBossState
{
    public MorsoraIdleState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName) : base(stateMachine, bossController, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        bossController.IncrementBasicAttackCounter();

        Debug.Log("basic attack counter is " + bossController.basicAttackCounter);
        

        if (bossController.basicAttackCounter == 0)
        {
            // Special attack cycle
            stateMachine.ChangeState(bossController.GetRandomSpecialAttackState());
        }
        else
        {
            // Basic attack cycle
            stateMachine.ChangeState(bossController.GetRandomBasicAttackState());
        }
    }

    public override void Update()
    {
        base.Update();

        //Debug.Log("timer is " + timer);
        if (timer > 5f)
        {
            MorsoraBossState randomState = bossController.GetRandomState();

            //stateMachine.ChangeState(randomState);
            //stateMachine.ChangeState(Random.value < 0.5f ? bossController.darkScytheSwingState : bossController.lightScytheSwingState);
            //stateMachine.ChangeState(bossController.radialTentacleSlamState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
