using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Trying to balance this so that its definitely easy to dodge, BUT the player NEEDS to stop shooting to be fast enough
public class MorsoraRadialTentacleSlamState : MorsoraBossState
{
    private int totalWaves = 3;
    private int currentWave = 0;
    private float timeBetweenWaves = 2.5f;
    private float waveTimer = 0f;
    private bool isWaiting = false;
    private float initialAngle;

    public MorsoraRadialTentacleSlamState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName)
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
        initialAngle = Random.Range(0f, 360f);
        SpawnWave();
    }

    private void SpawnWave()
    {


        // Spawn tentacles with a gap facing the player
        //bossController.spawnTentacleSnapAbility.SpawnCircleOfTentaclesWithGap(Random.Range(0f, 360f), 60f);

        bossController.spawnTentacleSnapAbility.SpawnCircleOfTentaclesWithGap(initialAngle + currentWave * Random.Range(160f, 200f), 50f);

        currentWave++;
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
                    // Transition back to idle when all waves are done
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
