using System.Collections;
using UnityEngine;

public class TimelordMeteorShowerState : TimelordBossState
{
    private float stateDuration = 10f;
    private float stateTimer = 0f;

    private float timeBetweenSets = 2f;
    private float setTimer = 0f;

    private int totalSetsToSpawn = 3;
    private int meteorsPerSet = 6;

    private int setsSpawned = 0;

    // NEW: For alternative single-by-single spawn
    private bool useOneByOneSpawning = false;    
    private float singleSpawnInterval = 0.2f;    
    private float singleSpawnTimer = 0f;        
    private int meteorsSpawnedThisSet = 0;      

    public TimelordMeteorShowerState(TimelordBossStateMachine stateMachine, TimelordBossController bossController)
        : base(stateMachine, bossController, "Idle")
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        stateTimer = stateDuration;
        setTimer = 0f;
        setsSpawned = 0;

        meteorsSpawnedThisSet = 0;
        singleSpawnTimer = 0f;

        useOneByOneSpawning = Random.Range(0, 2) == 0;

        if (!useOneByOneSpawning)
        {
            RunManager.Instance.StartCoroutine(SpawnMeteorSet(0)); //using RunManager because it's an easy-to-access monobehavior. Change if it causes any problems somehow.
            setsSpawned++;
        }
    }

    public override void Update()
    {
        base.Update();

        stateTimer -= Time.deltaTime;

        if (!useOneByOneSpawning)
        {

            setTimer -= Time.deltaTime;

            if (setTimer <= 0f && setsSpawned < totalSetsToSpawn)
            {
                RunManager.Instance.StartCoroutine(SpawnMeteorSet(setsSpawned));
                setsSpawned++;
            }

            if (setsSpawned >= totalSetsToSpawn && stateTimer <= 0f)
            {
                stateMachine.ChangeState(bossController.takeABreakState);
            }
        }
        else
        {

            singleSpawnTimer -= Time.deltaTime;

            if (singleSpawnTimer <= 0f)
            {
                SpawnSingleMeteor();
                meteorsSpawnedThisSet++;
                singleSpawnTimer = singleSpawnInterval; // reset the timer
            }

            if (meteorsSpawnedThisSet >= (totalSetsToSpawn * meteorsPerSet) || stateTimer <= 0f)
            {
                stateMachine.ChangeState(bossController.takeABreakState);
            }

        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator SpawnMeteorSet(int patternNumber)
    {
        setTimer = timeBetweenSets;
        float initialAngle = Random.Range(0f, 360f);

        for (int i = 0; i < meteorsPerSet/2; i++)
        {
            bossController.meteorSpawner.SpawnMeteorAtAngle(initialAngle + i * 25f);
            yield return new WaitForSeconds(0.1f);
        }

        //initialAngle += Random.Range(150f, 210f);
        initialAngle += Random.Range(70f, 90f);

        for (int i = meteorsPerSet/2; i < meteorsPerSet; i++)
        {
            bossController.meteorSpawner.SpawnMeteorAtAngle(initialAngle + i * 25f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SpawnSingleMeteor()
    {
        float randomAngle = Random.Range(0f, 360f);
        bossController.meteorSpawner.SpawnMeteorAtAngle(randomAngle);
    }
} 