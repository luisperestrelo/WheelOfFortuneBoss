using UnityEngine;

public class BossState
{
    protected BossStateMachine stateMachine;
    protected float timer = 0f;
    protected BossController bossController;

    public BossState(BossStateMachine stateMachine, BossController bossController)
    {
        this.stateMachine = stateMachine;
        this.bossController = bossController;
    }

    public virtual void Enter()
    {
        timer = 0f;
        Debug.Log(bossController.name + " Entered state: " + this.GetType().Name);
    }

    public virtual void Update()
    {
        timer += Time.deltaTime;
    }

    public virtual void Exit()
    {
        Debug.Log(bossController.name + " Exited state: " + this.GetType().Name);
    }
} 