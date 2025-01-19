using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraDarkScytheSwingState : MorsoraBossState
{
    private bool isPartOfCombo = false;
    private float normalSpeed = 1f;
    private float comboSpeed = 1f;  // idea is that its faster in combo
                                    // but without an obvious way to show the player the boss is doing the combo
                                    // it doesnt work. Revisit later

    public void SetAsCombo(bool isCombo)
    {
        isPartOfCombo = isCombo;
    }

    public MorsoraDarkScytheSwingState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName) : base(stateMachine, bossController, animBoolName)
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
                isPartOfCombo = false;
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
