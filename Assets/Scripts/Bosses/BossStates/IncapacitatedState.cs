using UnityEngine;

public class IncapacitatedState : BossState
{
    private float stateDuration;

    public IncapacitatedState(BossStateMachine stateMachine, BossController bossController, float duration) : base(stateMachine, bossController)
    {
        stateDuration = duration;
    }

    public override void Enter()
    {
        base.Enter();
        
        //bossController.StopAllCoroutines();
        bossController.StopFireSlashCoroutine();    
    }

    public override void Update()
    {
        base.Update();

        if (timer >= stateDuration)
        {
            // Transition to the next state determined by the state that initiated the IncapacitatedState
            stateMachine.TransitionToNextStateAfterIncapacitated();
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.StartFireSlashCoroutine();

    }

    public void SetDuration(float duration)
    {
        stateDuration = duration;
    }
} 