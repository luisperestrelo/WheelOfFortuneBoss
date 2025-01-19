using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraThrowChakramState : MorsoraBossState
{
    public MorsoraThrowChakramState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName) : base(stateMachine, bossController, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (isAnimationFinishedTriggerCalled)
        {
            stateMachine.ChangeState(bossController.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
