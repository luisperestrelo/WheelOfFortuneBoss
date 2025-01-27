using UnityEngine;

public class MinorCritBuff : BuffBase
{
    private Stats _targetStats;
    private int aggregatorId = -1;
    private float extraCrit; // e.g. 0.15f => +15% additive to base

    public MinorCritBuff(float extraCrit, float duration)
    {
        BuffId = "MinorCritBuff";
        BuffType = BuffType.Buff;
        StackingMode = StackingMode.ReplaceOldWithNew;
        this.extraCrit = extraCrit;
        Duration = duration;
    }

    public override void OnApply(Stats targetStats)
    {
        _targetStats = targetStats;
        // aggregatorId = targetStats.AddCritContribution(0.15f) => +15% 
        aggregatorId = _targetStats.AddCritContribution(extraCrit);
    }

    public override void OnUpdate(Stats targetStats, float deltaTime)
    {
    }

    public override void OnRemove(Stats targetStats)
    {
        if (_targetStats != null && aggregatorId >= 0)
        {
            _targetStats.RemoveCritContribution(aggregatorId);
            aggregatorId = -1;
        }
    }
} 