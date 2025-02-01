using UnityEngine;

public class FullCircleDamageBuff : BuffBase
{
    private Stats _targetStats;
    private bool isApplied = false;
    private int aggregatorId = -1;
    private float damageMultiplier = 1.2f; // +20% per stack

    public FullCircleDamageBuff(float damageMultiplier)
    {
        BuffId = "FullCircleDamageBuff";
        BuffType = BuffType.Buff;
        StackingMode = StackingMode.IndependentButRefreshesAll;
 
        Duration = 999999f;
        MaxStackCount = 5;
        this.damageMultiplier = damageMultiplier;
    }

    public override void OnApply(Stats targetStats)
    {
        _targetStats = targetStats;
        aggregatorId = _targetStats.AddDamageContribution(damageMultiplier);
        isApplied = true;
    }

    public override void OnUpdate(Stats targetStats, float deltaTime)
    {
    }

    public override void OnRemove(Stats targetStats)
    {
        if (isApplied && aggregatorId >= 0)
        {
            _targetStats.RemoveDamageContribution(aggregatorId);
            aggregatorId = -1;
            isApplied = false;
        }
    }
} 