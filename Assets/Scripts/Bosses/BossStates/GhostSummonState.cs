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

        // Example: These work a bit differently, can define the amount of ghosts in the LinearGhostsAbility script 
        //whereas for chasing ghosts it's just 1 per ability. All this can be refactored later
        spawnLinearGhostsAbility.SpawnGhosts();
        for (int i = 0; i < 3; i++)
        {
            spawnChasingGhostAbility.SpawnGhost();
        }
    }

    public override void Update()
    {
        base.Update();

        if (timer >= stateDuration)
        {
            stateMachine.ChangeState(stateMachine.throwSpearsState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
} 