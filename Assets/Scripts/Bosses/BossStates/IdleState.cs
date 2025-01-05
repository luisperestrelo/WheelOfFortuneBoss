using UnityEngine;
using System.Collections;

public class IdleState : BossState
{
    //it still shoots fire slashes in idle
    private float stateDuration = 5.5f; 

    public IdleState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController) { }

    public override void Enter()
    {
        base.Enter();
        bossController.StartCoroutine(TransitionToNextState());
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator TransitionToNextState()
    {
        yield return new WaitForSeconds(stateDuration);
        stateMachine.ChangeState(new FireFieldsState(stateMachine, bossController));
    }
} 