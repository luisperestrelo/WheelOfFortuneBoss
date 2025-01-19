using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraShockwavesState : MorsoraBossState
{
    private int shockwaveCount = 0;
    private int numberOfShockwaves = 3;

    public MorsoraShockwavesState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName) : base(stateMachine, bossController, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        bossController.DisableAllConstantAbilities();
        shockwaveCount = 0;
    }

    public override void Update()
    {
        base.Update();

        if (isAnimationFinishedTriggerCalled)
        {
            shockwaveCount++;
            isAnimationFinishedTriggerCalled = false;
            if (shockwaveCount >= numberOfShockwaves)
            {
                stateMachine.ChangeState(bossController.idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.RestartAllConstantAbilities();
    }
}
