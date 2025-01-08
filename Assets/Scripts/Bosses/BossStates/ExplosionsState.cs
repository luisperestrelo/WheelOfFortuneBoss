using UnityEngine;

public class ExplosionsState : BossState
{
    private RandomExplosionsAbility randomExplosionsAbility;
    private SpawnRangedMinionsAbility spawnRangedMinionsAbility;
    private float stateDuration = 10f;
    private float timeBetweenExplosionsSets = 2.5f;
    private float explosionsSetTimer = 0f;

    public ExplosionsState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController)
    {
        randomExplosionsAbility = bossController.GetComponent<RandomExplosionsAbility>();
        spawnRangedMinionsAbility = bossController.GetComponent<SpawnRangedMinionsAbility>();
    }

    public override void Enter()
    {
        base.Enter();

        if (randomExplosionsAbility == null)
        {
            Debug.LogError("RandomExplosionsAbility not found on BossController!");
            return;
        }

        randomExplosionsAbility.TriggerExplosions();
        spawnRangedMinionsAbility.SpawnMinions();
        
    }

    public override void Update()
    {
        base.Update();

        explosionsSetTimer += Time.deltaTime;

        if (explosionsSetTimer >= timeBetweenExplosionsSets)
        {
            randomExplosionsAbility.TriggerExplosions();
            
            explosionsSetTimer = 0f;
        }

        if (timer >= stateDuration)
        {
            stateMachine.ChangeState(stateMachine.randomShockwaveAndAbilityState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
} 