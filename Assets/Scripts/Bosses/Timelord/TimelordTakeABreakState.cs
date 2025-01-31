using UnityEngine;

public class TimelordTakeABreakState : TimelordBossState
{
    private float totalWaitTime;
    private float elapsedTime;

    public TimelordTakeABreakState(TimelordBossStateMachine stateMachine,
                                   TimelordBossController bossController,
                                   float waitTime)
        : base(stateMachine, bossController, "Idle")
    {
        totalWaitTime = waitTime;
    }

    public override void Enter()
    {
        base.Enter();
        elapsedTime = 0f;
    }

    public override void Update()
    {
        base.Update();
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= totalWaitTime)
        {
            // after waiting, go back to idle
            stateMachine.ChangeState(bossController.idleState);
        }
    }
} 