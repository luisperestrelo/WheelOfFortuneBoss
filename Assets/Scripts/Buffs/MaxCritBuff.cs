using UnityEngine;

public class MaxCritBuff : BuffBase
{
    private Stats _targetStats;
    private int aggregatorId = -1;
    private float extraCrit;  // Typically 1.0f => +100%

    /// <summary>
    /// Provide how much additive crit to apply (e.g. +1.0 => +100%) and how long it lasts.
    /// </summary>
    public MaxCritBuff(float extraCrit, float duration)
    {
        BuffId = "MaxCritBuff";
        BuffType = BuffType.Buff; 
        StackingMode = StackingMode.ReplaceOldWithNew;  
        this.extraCrit = extraCrit;
        Duration = duration;
    }

    public override void OnApply(Stats targetStats)
    {
        _targetStats = targetStats;
        // aggregatorId = targetStats.AddCritContribution(1.0f) => guaranteed crit
        aggregatorId = _targetStats.AddCritContribution(extraCrit);
    }

    public override void OnUpdate(Stats targetStats, float deltaTime)
    {
        // Nothing special each frame
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