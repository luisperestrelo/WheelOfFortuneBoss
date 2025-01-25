using UnityEngine;

public class CritBuff : BuffBase
{
    private float addedCrit;  // e.g. 1.0 => +100%
    private PlayerStats _targetStats;
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

    public override void OnApply(PlayerStats targetStats)
    {
        _targetStats = targetStats;
        aggregatorId = _targetStats.AddCritContribution(addedCrit);
        isApplied = true;
    }

    public override void OnUpdate(Health targetHealth, float deltaTime)
    {
        // No special tick logic
    }

    public override void OnRemove(PlayerStats targetStats)
    {
        if (isApplied && aggregatorId >= 0)
        {
            _targetStats.RemoveCritContribution(aggregatorId);
            aggregatorId = -1;
            isApplied = false;
        }
    }
} 