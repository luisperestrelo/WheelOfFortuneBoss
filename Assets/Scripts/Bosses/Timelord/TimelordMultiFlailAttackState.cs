using UnityEngine;

public class TimelordMultiFlailAttackState : TimelordBossState
{
    private float stateDuration = 5f;
    private float stateTimer = 0f;

    public TimelordMultiFlailAttackState(TimelordBossStateMachine stateMachine, TimelordBossController bossController)
        : base(stateMachine, bossController, "Idle")
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = stateDuration;
        bossController.flailAttack.SpawnFlail(0f);
        bossController.flailAttack.QuadSlamSequence();  
    }


    public override void Update()
    {
        base.Update();

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            stateMachine.ChangeState(bossController.CreateTakeABreakState(2f));
        }
        
    }


    public override void Exit()
    {
        base.Exit();
        bossController.flailAttack.DespawnFlail();
    }

} 