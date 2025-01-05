using UnityEngine;
using System.Collections;

public class IdleState : BossState
{
    private float stateDuration = 5.5f; 

    public IdleState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController) { }

    public override void Enter()
    {
        base.Enter();
        // Transition to the first state (e.g., FireFieldsState) after a short delay
        bossController.StartCoroutine(TransitionToNextState());
    }

    public override void Update()
    {
        base.Update();
        // Nothing to do here, just waiting for the transition
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