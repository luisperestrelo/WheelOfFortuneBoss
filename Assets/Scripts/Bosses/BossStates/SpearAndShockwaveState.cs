using UnityEngine;
using System.Collections;

public class SpearAndShockwaveState : BossState
{
    private ThrowSpearsAbility throwSpearsAbility;
    private ConcentricShockwavesAttack concentricShockwavesAttack;
    private float stateDuration = 10f;

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

        bossController.StartCoroutine(ThrowSpearSets());
        concentricShockwavesAttack.StartConcentricShockwaves(1f, true);
    }

    public override void Update()
    {
        base.Update();

        if (timer >= stateDuration)
        {
            stateMachine.ChangeState(stateMachine.laserSweepState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.StartFireSlashCoroutine();
    }


    private IEnumerator ThrowSpearSets()
    {
        throwSpearsAbility.ThrowSpears(4, false, 0f);
        yield return new WaitForSeconds(3f);

        throwSpearsAbility.ThrowSpears(4, false, 45f);
        yield return new WaitForSeconds(3f);

        throwSpearsAbility.ThrowSpears(4, false, 90f);
        yield return new WaitForSeconds(3f);    
        }


}