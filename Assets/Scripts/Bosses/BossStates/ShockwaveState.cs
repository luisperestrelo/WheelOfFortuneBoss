using UnityEngine;
using System.Collections;

public class ShockwaveState : BossState
{
    private ConcentricShockwavesAttack concentricShockwavesAttack;

    private float stateDuration = 10f;

    public ShockwaveState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController)
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
        concentricShockwavesAttack.StartConcentricShockwaves();
    }

    public override void Update()
    {
        base.Update();

        if (timer >= stateDuration)
        {
            stateMachine.ChangeState(stateMachine.ghostSummonState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.StartFireSlashCoroutine();
    }
}