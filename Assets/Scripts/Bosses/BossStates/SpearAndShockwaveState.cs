using UnityEngine;

public class SpearAndShockwaveState : BossState
{
    private ThrowSpearsAbility throwSpearsAbility;
    private ConcentricShockwavesAttack concentricShockwavesAttack;
    private float stateDuration = 4f;

    public SpearAndShockwaveState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController)
    {
        throwSpearsAbility = bossController.GetComponent<ThrowSpearsAbility>();
        concentricShockwavesAttack = bossController.GetComponent<ConcentricShockwavesAttack>();
    }

    public override void Enter()
    {
        base.Enter();
        bossController.StopFireSlashCoroutine();

        if (throwSpearsAbility == null || concentricShockwavesAttack == null)
        {
            Debug.LogError("Spear or shockwave abilities not found on BossController!");
            return;
        }

        // Example: Throw 4 spears and emit 1 shockwave
        throwSpearsAbility.ThrowSpears();
        concentricShockwavesAttack.StartConcentricShockwaves();
    }

    public override void Update()
    {
        base.Update();

        if (timer >= stateDuration)
        {
            stateMachine.ChangeState(new LaserSweepState(stateMachine, bossController));
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.StartFireSlashCoroutine();
    }
} 