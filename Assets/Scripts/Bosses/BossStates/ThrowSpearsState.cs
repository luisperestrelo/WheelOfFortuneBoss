using UnityEngine;
using System.Collections;

public class ThrowSpearsState : BossState
{
    private ThrowSpearsAbility throwSpearsAbility;
    private float stateDuration = 8f;

    public ThrowSpearsState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController)
    {
        throwSpearsAbility = bossController.GetComponent<ThrowSpearsAbility>();
    }

    public override void Enter()
    {
        base.Enter();

        if (throwSpearsAbility == null)
        {
            Debug.LogError("ThrowSpearsAbility not found on BossController!");
            return;
        }

        bossController.StartCoroutine(ThrowSpearSets());
    }

    public override void Update()
    {
        base.Update();

        if (timer >= stateDuration)
        {
            stateMachine.RequestStateChange(stateMachine.minionSummonState); // Changed to RequestStateChange
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    // first set static, then 2 randoms, then one static. Idk can jsut throw random numbers in here
    private IEnumerator ThrowSpearSets()
    {
        throwSpearsAbility.ThrowSpears(4, false, 0f);
        yield return new WaitForSeconds(2f);

        throwSpearsAbility.ThrowSpears(4, true, 0f);
        yield return new WaitForSeconds(2f);

        throwSpearsAbility.ThrowSpears(4, true, 0f);
        yield return new WaitForSeconds(2f);

        throwSpearsAbility.ThrowSpears(5, false, 45f);
    }
} 