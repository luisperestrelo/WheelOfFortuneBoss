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

        // Example: Start a clockwise sweep for 5 seconds
        circularSweepAttack.StartCircularSweep(0f, true, circularSweepAttack.GetDefaultLaserDuration(), circularSweepAttack.GetDefaultLaserRotationSpeed());
    }

    public override void Update()
    {
        base.Update();

        if (timer >= stateDuration)
        {
            stateMachine.ChangeState(new MinionSummonState(stateMachine, bossController));
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
} 