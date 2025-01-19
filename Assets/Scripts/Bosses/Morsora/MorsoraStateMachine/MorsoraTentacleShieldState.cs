using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraTentacleShieldState : MorsoraBossState
{
    private WheelManager wheelManager;
    private BossHealth bossHealth;
    private TentacleShieldBusterField shieldBusterField; // Assign this in the inspector or find it dynamically
    private int chargesNeededToBreakShield = 2; // Set this to the desired number of charges
    private int chargesCompleted = 0;
    private int currentFieldIndex;

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
    }

    public override void Update()
    {
        base.Update();
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
            // Shield break condition met
            stateMachine.ChangeState(bossController.idleState);
        }
        else
        {
            // Add a new field
            AddFieldToWheel();
        }
    }
}
