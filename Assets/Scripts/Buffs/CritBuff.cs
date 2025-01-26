using UnityEngine;

public class CritBuff : BuffBase
{
    private float addedCrit;  // e.g. 1.0 => +100%
    private Stats _targetStats;
    private int aggregatorId = -1;
    private bool isApplied = false;

    public CritBuff(float addedCrit, float duration)
    {
        BuffId = "CritBuff";
        BuffType = BuffType.Buff; 
        StackingMode = StackingMode.ReplaceOldWithNew; 
        Duration = duration;
        this.addedCrit = addedCrit;
    }

    public override void OnApply(Stats targetStats)
    {
        _targetStats = targetStats;
        aggregatorId = _targetStats.AddCritContribution(addedCrit);
        isApplied = true;
    }

    public override void OnUpdate(Stats targetStats, float deltaTime)
    {
    }

    public override void OnRemove(Stats targetStats)
    {
        if (isApplied && aggregatorId >= 0)
        {
            _targetStats.RemoveCritContribution(aggregatorId);
            aggregatorId = -1;
            isApplied = false;
        }
    }
} 