using UnityEngine;

public class LaserSweepState : BossState
{
    private CircularSweepAttack circularSweepAttack;
    private float stateDuration = 5f;

    public LaserSweepState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController)
    {
        circularSweepAttack = bossController.GetComponent<CircularSweepAttack>();
    }

    public override void Enter()
    {
        base.Enter();

        if (circularSweepAttack == null)
        {
            Debug.LogError("CircularSweepAttack not found on BossController!");
            return;
        }

        circularSweepAttack.StartCircularSweep(-30f, true, 5.2f, circularSweepAttack.GetDefaultLaserRotationSpeed());
        circularSweepAttack.StartCircularSweep(30f, true, 5.2f, circularSweepAttack.GetDefaultLaserRotationSpeed());
    }

    public override void Update()
    {
        base.Update();

        if (timer >= stateDuration)
        {
            stateMachine.RequestStateChange(stateMachine.explosionsState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}