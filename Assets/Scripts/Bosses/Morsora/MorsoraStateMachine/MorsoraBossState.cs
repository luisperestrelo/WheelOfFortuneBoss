using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraBossState
{
    protected MorsoraBossStateMachine stateMachine;
    protected MorsoraBossController bossController;

    protected bool isAnimationFinishedTriggerCalled;
    protected float timer = 0f;

    private string animBoolName;

    public MorsoraBossState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.bossController = bossController;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        isAnimationFinishedTriggerCalled = false;
        timer = 0f;
        bossController.anim.SetBool(animBoolName, true);
        Debug.Log("Entering state " + this);
    }

    public virtual void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log ("Boss current state is " + this);
    }

    public virtual void Exit()
    {
        Debug.Log("Exiting state " + this);
        bossController.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        isAnimationFinishedTriggerCalled = true;
    }




}
