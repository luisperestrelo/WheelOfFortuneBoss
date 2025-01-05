using UnityEngine;

public class RandomShockwaveAndAbilityState : BossState
{
    private ConcentricShockwavesAttack concentricShockwavesAttack;
    private float stateDuration = 8f;

    public RandomShockwaveAndAbilityState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController)
    {
        concentricShockwavesAttack = bossController.GetComponent<ConcentricShockwavesAttack>();
    }

    public override void Enter()
    {
        base.Enter();
        bossController.StopFireSlashCoroutine();

        if (concentricShockwavesAttack == null)
        {
            Debug.LogError("ConcentricShockwavesAttack not found on BossController!");
            return;
        }

        bool expandOutward = Random.Range(0, 2) == 0; // we'll try this later, lets just have it always be concentric so its different
        float startRadius =0f;

        expandOutward = false;

        if (!expandOutward)
            startRadius = 30f;
        concentricShockwavesAttack.StartConcentricShockwaves(startRadius, expandOutward);


        TriggerRandomAbility();
    }

    public override void Update()
    {
        base.Update();

        if (timer >= stateDuration)
        {
            stateMachine.ChangeState(new IdleState(stateMachine, bossController));
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.StartFireSlashCoroutine();
    }

    private void TriggerRandomAbility()
    {
        int randomIndex = Random.Range(0, 8);

        switch (randomIndex)
        {
            case 0:
                bossController.GetComponent<SpawnChasingGhostAbility>()?.SpawnGhost();
                bossController.GetComponent<SpawnChasingGhostAbility>()?.SpawnGhost();
                bossController.GetComponent<SpawnChasingGhostAbility>()?.SpawnGhost();
                break;
            case 1:
                bossController.GetComponent<ThrowSpearsAbility>()?.ThrowSpears(5, true, 0f);
                break;
            case 2:
                bossController.GetComponent<SpawnRangedMinionsAbility>()?.SpawnMinions();
                break;
            case 3:
                bossController.GetComponent<RandomExplosionsAbility>()?.TriggerExplosions();
                break;
            case 4:
                bossController.GetComponent<SpawnRadialGhostsAbility>()?.SpawnGhosts();
                break;
            case 5:
                bossController.GetComponent<CircularSweepAttack>()?.StartCircularSweep(0f, true, 5f, 100f);
                break;
            case 6:
                bossController.GetComponent<CircularSweepAttack>()?.StartCircularSweep(0f, false, 5f, 100f);
                break;
            case 7:
                bossController.GetComponent<SpawnLinearGhostsAbility>()?.SpawnGhosts();
                break;
            default:
                Debug.LogWarning("Invalid random ability index in RandomShockwaveAndAbilityState");
                break;
        }
    }
}