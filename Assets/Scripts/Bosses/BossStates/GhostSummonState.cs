using UnityEngine;

public class GhostSummonState : BossState
{
    private SpawnLinearGhostsAbility spawnLinearGhostsAbility;
    private SpawnChasingGhostAbility spawnChasingGhostAbility;
    private float stateDuration = 5f;

    public GhostSummonState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController)
    {
        spawnLinearGhostsAbility = bossController.GetComponent<SpawnLinearGhostsAbility>();
        spawnChasingGhostAbility = bossController.GetComponent<SpawnChasingGhostAbility>();
    }

    public override void Enter()
    {
        base.Enter();
        if (spawnLinearGhostsAbility == null || spawnChasingGhostAbility == null)
        {
            Debug.LogError("Ghost spawning abilities not found on BossController!");
            return;
        }

        // Example: Spawn 3 linear ghosts and 2 chasing ghosts
        spawnLinearGhostsAbility.SpawnGhosts();
        for (int i = 0; i < 5; i++)
        {
            spawnChasingGhostAbility.SpawnGhost();
        }
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