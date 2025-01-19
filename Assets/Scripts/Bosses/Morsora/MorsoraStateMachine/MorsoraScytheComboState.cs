using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraScytheComboState : MorsoraBossState
{
    private int totalSwings = 6;
    private int currentSwing = 0;
    private float lightScytheChance = 0.5f;
    private float chanceAdjustment = 0.25f;

    public MorsoraScytheComboState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName)
        : base(stateMachine, bossController, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();


        if (currentSwing >= totalSwings)
        {
            // If we've completed all swings, go back to idle and reset the swing counter   
            currentSwing = 0;
            stateMachine.ChangeState(bossController.idleState);
            return;
        }


        PerformNextSwing();
    }

    private void PerformNextSwing()
    {
        currentSwing++;



        Debug.Log("Current swing: " + currentSwing);
        // Choose light or dark scythe based on current probability
        if (Random.value < lightScytheChance)
        {
            bossController.lightScytheSwingState.SetAsCombo(true);
            stateMachine.ChangeState(bossController.lightScytheSwingState);

            // Decrease light scythe chance, increase dark scythe chance
            lightScytheChance -= chanceAdjustment;
        }
        else
        {
            bossController.darkScytheSwingState.SetAsCombo(true);
            stateMachine.ChangeState(bossController.darkScytheSwingState);

            // Increase light scythe chance, decrease dark scythe chance
            lightScytheChance += chanceAdjustment;
        }

        lightScytheChance = Mathf.Clamp(lightScytheChance, 0f, 1f);

        if (lightScytheChance == 0f || lightScytheChance == 1f)
        {
            lightScytheChance = 0.5f;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
