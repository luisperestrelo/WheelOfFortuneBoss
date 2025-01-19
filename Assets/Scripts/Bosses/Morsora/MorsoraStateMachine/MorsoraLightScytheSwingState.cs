using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraLightScytheSwingState : MorsoraBossState
{
    private bool isPartOfCombo = false;
    private float normalSpeed = 1f;
    private float comboSpeed = 1f;  // idea is that its faster in combo

    public void SetAsCombo(bool isCombo)
    {
        isPartOfCombo = isCombo;
    }

    public MorsoraLightScytheSwingState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName) : base(stateMachine, bossController, animBoolName)
    {
    }   

    public override void Enter()
    {
        base.Enter();
        
        bossController.anim.speed = isPartOfCombo ? comboSpeed : normalSpeed;
    }

    public override void Update()
    {
        base.Update();

        if (isAnimationFinishedTriggerCalled)
        {
            if (isPartOfCombo)
            {
                isPartOfCombo = false; // Reset for next time
                stateMachine.ChangeState(bossController.scytheComboState);
            }
            else
            {
                stateMachine.ChangeState(bossController.idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.anim.speed = normalSpeed;
    }
}
