using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraTentacleShieldState : MorsoraBossState
{
    private WheelManager wheelManager;
    private BossHealth bossHealth;
    private TentacleShieldBusterField shieldBusterField; // Assign this in the inspector or find it dynamically
    private int chargesNeededToBreakShield = 1; // Set this to the desired number of charges
    private int chargesCompleted = 0;
    private int currentFieldIndex;

    private float minionSpawnInterval = 5f; // Initial time between minion spawns
    private float minionSpawnTimer = 0f;
    private float spawnRateIncreaseFactor = 0.02f; // Controls how quickly the spawn rate increases
                                                // this is to prevent the player from abusing this phase
                                                // however it might make it too hard for a "noob" player
                                                // and it's probably not even needed to think of an edge-case like this
                                                // but yeah, can just set to 0 if need.

    public MorsoraTentacleShieldState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName, TentacleShieldBusterField shieldBusterField) : base(stateMachine, bossController, animBoolName)
    {
        wheelManager = bossController.wheelManager;
        bossHealth = bossController.GetComponent<BossHealth>();
        this.shieldBusterField = shieldBusterField;
        Debug.Log("Shield Buster Field: " + shieldBusterField);
        currentFieldIndex = -1;
    }

    public override void Enter()
    {
        base.Enter();
        bossController.DisableAllConstantAbilities();
        bossHealth.SetImmune(true);
        bossController.spawnTentacleShield.SpawnShield();

        chargesCompleted = 0;

        // Add the special field to the wheel
        AddFieldToWheel();

        // Reset timers
        minionSpawnTimer = 2f; // spawn first set after 3 seconds
        timer = 0f;
    }

    public override void Update()
    {
        base.Update();

        // Update timers
        minionSpawnTimer += Time.deltaTime;

        // Minion spawning logic
        if (minionSpawnTimer >= minionSpawnInterval)
        {
            bossController.spawnRangedMinions.SpawnMinions();
            minionSpawnTimer = 0f;

            // Increase spawn rate (decrease interval)
            minionSpawnInterval -= spawnRateIncreaseFactor * timer;

            // Set a minimum spawn interval to prevent it from becoming too fast
            minionSpawnInterval = Mathf.Max(minionSpawnInterval, 1.5f);

            Debug.Log("Minion Spawn Interval: " + minionSpawnInterval);
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.RestartAllConstantAbilities();
        bossHealth.SetImmune(false);
        bossController.spawnTentacleShield.DespawnShield();

        // Remove any remaining special fields
        RemoveAllShieldBusterFields();
    }

    private void AddFieldToWheel()
    {
        // Add the special field at a random index, excluding the current index
        currentFieldIndex = wheelManager.AddFieldAtRandomIndexExcept(shieldBusterField, currentFieldIndex);
    }

    private void RemoveAllShieldBusterFields()
    {
        // Remove all instances of the special field from the wheel
        while (wheelManager.ContainsField(shieldBusterField))
        {
            wheelManager.RemoveField(shieldBusterField);
        }
        currentFieldIndex = -1;
    }

    public void IncrementChargeCount()
    {
        wheelManager.RemoveField(currentFieldIndex);
        chargesCompleted++;
        if (chargesCompleted >= chargesNeededToBreakShield)
        {
            bossController.StartPhase2();

            stateMachine.ChangeState(bossController.idleState);
        }
        else
        {
            // Add a new field
            AddFieldToWheel();
        }
    }
}
