using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraTentacleHellState : MorsoraBossState
{
    private float stateDuration = 12f;  // Total duration of the state
    private float cardinalInterval = 1.5f;  
    private float randomInterval = .4f;   // Time between random tentacle spawns
    private float cardinalTimer = 0f;
    private float randomTimer = 0f;
    private float initialTimer = 0f;



    public MorsoraTentacleHellState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName)
        : base(stateMachine, bossController, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        cardinalTimer = 0f;
        randomTimer = 0f;
        initialTimer = 0f;
        bossController.DisableAllConstantAbilities();
        //bossController.spawnFatTentacleAbility.SpawnFatTentacle(Random.Range(0f, 360f));
        //bossController.spawnFatTentacleAbility.SpawnFatTentacle180DegreesFromPlayer();
        float spawnedAngle = bossController.spawnFatTentacleAbility.SpawnFatTentacleDegreesFromPlayer(225f);
        //bossController.spawnFatTentacleAbility.SpawnFatTentacle(spawnedAngle+180f); // (maybe a 2nd tentacle for p2)

    }

    public override void Update()
    {
        base.Update();

        cardinalTimer += Time.deltaTime;
        randomTimer += Time.deltaTime;
        initialTimer += Time.deltaTime;

        /*         // Spawn cardinal tentacles
                if (cardinalTimer >= cardinalInterval)
                {
                    bossController.spawnTentacleSnapAbility.SpawnTentaclesAtCardinals(1);
                    cardinalTimer = 0f;
                } */

        // Spawn random tentacles
        /*         if (randomTimer >= randomInterval)
                {
                    bossController.spawnTentacleSnapAbility.SpawnTentaclesRandomlyAroundPlayerWithDelay(10, 1, 15f, 0.25f);
                    randomTimer = 0f;
                } */

        if (initialTimer >= 2f)
        {
            if (randomTimer >= randomInterval)
            {
                //random at player or random angle
                if (Random.value < 0.33f)
                {
                    bossController.spawnTentacleSnapAbility.SpawnTentacleSnapAtPlayer();
                }
                else
                {
                    bossController.spawnTentacleSnapAbility.SpawnTentacleSnap(Random.Range(0f, 360f), 1);
                }
                randomTimer = 0f;
            }
        }


        // Exit state after duration
        if (timer >= stateDuration)
        {
            stateMachine.ChangeState(bossController.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.RestartAllConstantAbilities();
        AbilityObjectManager.Instance.DestroyAllFatTentacles(); // TODO: Despawn animation
    }
}
