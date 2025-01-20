using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//similar to phase 1, but spawns 2 fat tentacles and the tentacle snaps are within the area the player got "locked in" 
public class MorsoraTentacleHellPhaseTwoState : MorsoraBossState
{
    private float stateDuration = 12f;  // Total duration of the state
    private float cardinalInterval = 1.5f;
    private float randomInterval = .4f;   // Time between random tentacle spawns
    private float cardinalTimer = 0f;
    private float randomTimer = 0f;
    private float initialTimer = 0f;
    private float fatTentacleSpawnedAngle = 0f;
    private float lockedInStartAngle;
    private float lockedInEndAngle;
    private PlayerSpinMovement playerSpinMovement;

    public MorsoraTentacleHellPhaseTwoState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName)
        : base(stateMachine, bossController, animBoolName)
    {
        playerSpinMovement = bossController.player.GetComponent<PlayerSpinMovement>();
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
        fatTentacleSpawnedAngle = bossController.spawnFatTentacleAbility.SpawnFatTentacleDegreesFromPlayer(225f);
        bossController.spawnFatTentacleAbility.SpawnFatTentacle(fatTentacleSpawnedAngle+180f); // (maybe a 2nd tentacle for p2)
        Debug.Log("Fat tentacle spawned at angle: " + fatTentacleSpawnedAngle + " " + (fatTentacleSpawnedAngle + 180f));
        // Determine the "locked-in" area, considering player's position
        float playerAngle = playerSpinMovement.CurrentAngle;
        lockedInStartAngle = fatTentacleSpawnedAngle;
        lockedInEndAngle = (fatTentacleSpawnedAngle + 180f) % 360f;

        // Check if the player is within the initial range
        if (!IsAngleWithinRange(playerAngle, lockedInStartAngle, lockedInEndAngle))
        {
            // Swap the start and end angles if the player is outside the initial range
            float temp = lockedInStartAngle;
            lockedInStartAngle = lockedInEndAngle;
            lockedInEndAngle = temp;
        }
    }

    public override void Update()
    {
        base.Update();

        cardinalTimer += Time.deltaTime;
        randomTimer += Time.deltaTime;
        initialTimer += Time.deltaTime;

        float playerCurrentAngle = playerSpinMovement.CurrentAngle;

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
                //random at player
                if (Random.value < 0.33f)
                {
                    //bossController.spawnTentacleSnapAbility.SpawnTentacleSnapAtPlayer();
                    if (IsAngleWithinRange(playerCurrentAngle, lockedInStartAngle, lockedInEndAngle))
                    {
                        bossController.spawnTentacleSnapAbility.SpawnTentacleSnapAtPlayer();
                    }
                    else
                    {
                        SpawnTentacleSnapWithinRange();
                    }
                }
                else
                {
                    //bossController.spawnTentacleSnapAbility.SpawnTentacleSnap(Random.Range(0f, 360f), 1);
                    SpawnTentacleSnapWithinRange();
                }
                randomTimer = 0f;
            }
        }


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

    private void SpawnTentacleSnapWithinRange()
    {
        float randomAngle = Random.Range(lockedInStartAngle, lockedInEndAngle);
        // Adjust if the range crosses the 0/360 boundary
        if (lockedInStartAngle > lockedInEndAngle)
        {
            if (Random.value < 0.5f)
            {
                randomAngle = Random.Range(lockedInStartAngle, 360f);
            }
            else
            {
                randomAngle = Random.Range(0f, lockedInEndAngle);
            }
        }
        bossController.spawnTentacleSnapAbility.SpawnTentacleSnap(randomAngle, 1);
    }

    private bool IsAngleWithinRange(float angle, float start, float end)
    {
        angle = (angle % 360 + 360) % 360;
        start = (start % 360 + 360) % 360;
        end = (end % 360 + 360) % 360;

        if (start <= end)
        {
            return angle >= start && angle <= end;
        }
        else
        {
            return angle >= start || angle <= end;
        }
    }
}
