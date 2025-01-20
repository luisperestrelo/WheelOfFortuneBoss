using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraTentacleSpiralState : MorsoraBossState
{
    private int totalWaves = 2;
    private int currentWave = 0;
    private float timeBetweenWaves = 5.5f;
    private float waveTimer = 0f;
    private bool isWaiting = false;
    private float initialAngle;
    private SpawnDirection spiralDirection;

    public MorsoraTentacleSpiralState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName)
        : base(stateMachine, bossController, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        bossController.DisableAllConstantAbilities();
        currentWave = 0;
        waveTimer = 0f;
        isWaiting = false;
        spiralDirection = Random.value < 0.5f ? SpawnDirection.Clockwise : SpawnDirection.CounterClockwise;
        SpawnWave();
    }

    private void SpawnWave()
    {
        // Spawn tentacles in a spiral pattern
        initialAngle = Random.Range(0f, 360f);
        bossController.spawnTentacleSnapAbility.SpawnTentacleSpiral(initialAngle, 0.1f, spiralDirection);

        currentWave++;
        spiralDirection = spiralDirection == SpawnDirection.Clockwise ? SpawnDirection.CounterClockwise : SpawnDirection.Clockwise; // Change direction for the next wave
        isWaiting = true;
        waveTimer = 0f;
    }

    public override void Update()
    {
        base.Update();

        if (isWaiting)
        {
            waveTimer += Time.deltaTime;

            if (waveTimer >= timeBetweenWaves)
            {
                isWaiting = false;

                if (currentWave < totalWaves)
                {
                    SpawnWave();
                }
                else
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
