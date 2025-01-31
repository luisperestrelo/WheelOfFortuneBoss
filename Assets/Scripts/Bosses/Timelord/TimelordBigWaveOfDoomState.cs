using UnityEngine;

public class TimelordBigWaveOfDoomState : TimelordBossState
{
    private float initialWaitTime = 4f;
    private float waitTimer = 0f;
    private bool hasWaitFinished = false;

    // After wave begins, how long it should last before going to Break
    private float waveDuration = 22f;
    private float waveTimer = 0f;
    private bool waveHasStarted = false;

    public TimelordBigWaveOfDoomState(TimelordBossStateMachine stateMachine, TimelordBossController bossController)
        : base(stateMachine, bossController, "Idle")
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        bossController.DisableAllConstantAbilities();
        
        waitTimer = initialWaitTime;
        hasWaitFinished = false;

        waveTimer = waveDuration;
        waveHasStarted = false;
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

                waveHasStarted = true;
                bossController.controlPortals.TogglePortals();
                bossController.startBigWaveOfDoom.StartWave();
            }
        }
        else
        {
            if (waveHasStarted)
            {
                waveTimer -= Time.deltaTime;
                if (waveTimer <= 0f)
                {
                    stateMachine.ChangeState(bossController.CreateTakeABreakState(1f));
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.RestartAllConstantAbilities();
        bossController.controlPortals.TogglePortals();
    }
} 