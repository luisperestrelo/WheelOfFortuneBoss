using UnityEngine;

/// <summary>
/// Each instance adds +7% (1.07) to damage aggregator. 
/// We can limit stacks to 10 in BuffManager or in the buff's OnApply.
/// This demonstration uses aggregator approach instead of direct math.
/// </summary>
public class StackingDamageBuff : BuffBase
{
    private float multiplierPerStack;
    private Stats _targetStats;
    private int aggregatorId = -1;
    private bool isApplied = false;

    public StackingDamageBuff(float multiplier, float duration)
    {
        BuffId = "StackingDamageBuff";
        BuffType = BuffType.Buff;
        StackingMode = StackingMode.Independent; 
        Duration = duration;

        // e.g. 1.07 => +7%
        multiplierPerStack = multiplier;

        MaxStackCount = 10;
    }

    public override void OnApply(Stats targetStats)
    {
        _targetStats = targetStats;
        aggregatorId = _targetStats.AddDamageContribution(multiplierPerStack);
        isApplied = true;
    }

    public override void OnUpdate(Stats targetStats, float deltaTime)
    {
        // No tick-based logic, so do nothing here
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