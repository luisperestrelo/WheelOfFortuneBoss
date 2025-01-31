using UnityEngine;

public class TimelordRealityRendState : TimelordBossState
{
    private float initialWaitTime = 4f;
    private float waitTimer = 0f;
    private bool hasWaitFinished = false;

    // After the rend has spawned, we'll use the rend's duration + extra seconds
    // to decide when to return to Idle.
    private float extraSeconds = 3f;
    private float rendTimer = 0f;
    private bool isRending = false;

    public TimelordRealityRendState(TimelordBossStateMachine stateMachine, TimelordBossController bossController)
        : base(stateMachine, bossController, "Idle")
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        bossController.DisableAllConstantAbilities();

        waitTimer = initialWaitTime;
        hasWaitFinished = false;

        rendTimer = 0f;
        isRending = false;
    }

    public override void Update()
    {
        base.Update();

        if (!hasWaitFinished)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                hasWaitFinished = true;
                bool isFastRend = Random.Range(0, 2) == 0;
                bossController.spawnRealityRend.SpawnRealityRendObject(isFastRend);

                float rendDuration = bossController.spawnRealityRend.GetRealityRendDuration();

                rendTimer = rendDuration + extraSeconds;
                isRending = true;
            }
        }
        else
        {
            if (isRending)
            {
                rendTimer -= Time.deltaTime;
                if (rendTimer <= 0f)
                {
                    stateMachine.ChangeState(bossController.idleState);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        bossController.RestartAllConstantAbilities();
    }
} 