using UnityEngine;

public class MinionSummonState : BossState
{
    private SpawnRangedMinionsAbility spawnRangedMinionsAbility;
    private float stateDuration = 5f;

    public MinionSummonState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController)
    {
        spawnRangedMinionsAbility = bossController.GetComponent<SpawnRangedMinionsAbility>();
    }

    public override void Enter()
    {
        base.Enter();

        if (spawnRangedMinionsAbility == null)
        {
            Debug.LogError("SpawnRangedMinionsAbility not found on BossController!");
            return;
        }

        // Example: Spawn 3 ranged minions
        spawnRangedMinionsAbility.SpawnMinions();
    }

    public override void Update()
    {
        base.Update();

        if (timer >= stateDuration)
        {
            stateMachine.ChangeState(new FireFieldsState(stateMachine, bossController));
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
} 