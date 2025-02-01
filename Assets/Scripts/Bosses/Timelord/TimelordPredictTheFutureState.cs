using UnityEngine;

public class TimelordPredictTheFutureState : TimelordBossState
{
    private float initialWaitTime = 4f;
    private float waitTimer = 0f;
    private bool hasWaitFinished = false;

    private int timesToPredict = 3;
    private float predictDelay = 10f;
    private float predictTimer = 0f;

    private int predictionsDone = 0;
    private bool isPredicting = false;

    // After we have spawned all predictions, we still want to wait out the last predictDelay
    private bool allPredictionsSpawned = false;

    // Start at 50% Real. If we pick Real, we shift realProbability DOWN by deltaProbability;
    // if we pick Fake, we shift realProbability UP by deltaProbability.
    // That way, two picks in a row sets probability = 0% or 100%, enforcing a switch next time.
    private float realProbability = 0.5f;
    private const float deltaProbability = 0.3f;

    public TimelordPredictTheFutureState(TimelordBossStateMachine stateMachine, TimelordBossController bossController)
        : base(stateMachine, bossController, "Idle")
    {
    }

    public override void Enter()
    {
        base.Enter();

        //bossController.DisableAllConstantAbilities();
        bossController.hourglass.gameObject.SetActive(true);

        waitTimer = initialWaitTime;
        hasWaitFinished = false;

        predictionsDone = 0;
        isPredicting = false;
        predictTimer = 0f;
        realProbability = 0.5f;
        allPredictionsSpawned = false; // We haven't finished spawning yet
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

                isPredicting = true;
                predictTimer = 0f;  // so the first prediction occurs immediately
            }
        }
        else
        {
            if (isPredicting)
            {
                predictTimer -= Time.deltaTime;

                // Spawn next prediction if it's time and we haven't used them all
                if (predictTimer <= 0f && !allPredictionsSpawned)
                {
                    DoPredict();
                    predictionsDone++;
                    predictTimer = predictDelay; // reset for the next predict-delay

                    // If we've spawned them all, mark that
                    if (predictionsDone >= timesToPredict)
                    {
                        allPredictionsSpawned = true;
                    }
                }
                // If all predictions have been spawned, wait out the final delay
                else if (allPredictionsSpawned)
                {
                    // Once the final timer has passed, we transition away
                    if (predictTimer <= 0f)
                    {
                        bossController.hourglass.gameObject.SetActive(false);
                        stateMachine.ChangeState(bossController.CreateTakeABreakState(3f));
                    }
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        bossController.hourglass.SetDefaultValues();
        bossController.hourglass.gameObject.SetActive(false);
        //bossController.RestartAllConstantAbilities();
    }

    // Picking Real pushes realProbability down by deltaProbability;
    // picking Fake pushes realProbability up. After two same picks in a row, realProbability
    // hits 0 or 1, guaranteeing a flip on the next pick.
    private void DoPredict()
    {
        Debug.Log("realProbability is " + realProbability);
        float roll = Random.value;
        bool doReal = (roll <= realProbability);


        if (doReal)
        {
            bossController.predictTheFutureAbility.RealFuture();
            // Shift the probability AWAY from Real
            realProbability = Mathf.Clamp(realProbability - deltaProbability, 0f, 1f);
        }
        else
        {
            bossController.predictTheFutureAbility.FakeFuture();
            // Shift the probability AWAY from Fake (thus raising Real)
            realProbability = Mathf.Clamp(realProbability + deltaProbability, 0f, 1f);
        }
    }
}