using UnityEngine;

public class MinionSummonState : BossState
{
    private SpawnRangedMinionsAbility spawnRangedMinionsAbility;
    private SpawnRadialGhostsAbility spawnRadialGhostsAbility;
    private float stateDuration = 15f;

    public MinionSummonState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController)
    {
        spawnRangedMinionsAbility = bossController.GetComponent<SpawnRangedMinionsAbility>();
        spawnRadialGhostsAbility = bossController.GetComponent<SpawnRadialGhostsAbility>();
    }

    public override void Enter()
    {
        base.Enter();

        if (spawnRangedMinionsAbility == null)
        {
            Debug.LogError("SpawnRangedMinionsAbility not found on BossController!");
            return;
        }

        
        spawnRangedMinionsAbility.SpawnMinions();
        spawnRadialGhostsAbility.SpawnGhosts();
    }

    public override void Update()
    {
        base.Update();

        if (timer >= stateDuration)
        {
            stateMachine.ChangeState(new SpearAndShockwaveState(stateMachine, bossController));
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
} 