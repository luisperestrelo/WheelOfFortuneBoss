using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraShockwaveWithTentaclesState : MorsoraBossState
{

    private int shockwaveCount = 0;
    private int numberOfShockwaves = 3;
    private SpawnTentacleSnapAbility spawnTentacleSnapAbility;


    public MorsoraShockwaveWithTentaclesState(MorsoraBossStateMachine stateMachine, MorsoraBossController bossController, string animBoolName) : base(stateMachine, bossController, animBoolName)
    {
        spawnTentacleSnapAbility = bossController.GetComponent<SpawnTentacleSnapAbility>();
    }

    public override void Enter()
    {
        base.Enter();
        bossController.DisableAllConstantAbilities();
        shockwaveCount = 0;
    }

    public override void Update()
    {
        base.Update();

        if (isAnimationFinishedTriggerCalled)
        {
            shockwaveCount++;
            isAnimationFinishedTriggerCalled = false;
            if (shockwaveCount >= numberOfShockwaves)
            {
                stateMachine.ChangeState(bossController.idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.RestartAllConstantAbilities();
    }
}
