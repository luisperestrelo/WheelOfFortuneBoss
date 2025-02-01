using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLordRotatingOrbsState : TimelordBossState
{
    private bool lasersEnabled = false;
    private float nextStateTime = 6f; // Time after which you move to the next state
    private float delayUntilLasersEnabled = 3f;

    public TimeLordRotatingOrbsState(TimelordBossStateMachine stateMachine, TimelordBossController bossController)
        : base(stateMachine, bossController, "Idle")
    {
    }

    public override void Enter()
    {
        base.Enter();
        bossController.DisableAllConstantAbilities();
        bossController.enableRotatingOrbs.EnableOrbHolder();
    }


    public override void Update()
    {
        base.Update();
        
        if (!lasersEnabled && timer >= delayUntilLasersEnabled)
        {
            bossController.enableRotatingOrbs.ToggleLasers(true);
            lasersEnabled = true;
        }

        if (timer >= nextStateTime)
        {
            stateMachine.ChangeState(bossController.CreateTakeABreakState(3f));
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.enableRotatingOrbs.DisableOrbHolder();
        bossController.RestartAllConstantAbilities();
        lasersEnabled = false;

    }
}



