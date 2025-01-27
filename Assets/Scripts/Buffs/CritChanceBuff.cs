using UnityEngine;

public class CritChanceBuff : BuffBase
{
    private Stats _targetStats;
    private bool _isApplied = false;
    private int _aggregatorId = -1;

    private float critIncrease;
    private float duration;

    public CritChanceBuff(float critIncrease, float duration)
    {
        BuffId = "CritChanceBuff";
        BuffType = BuffType.Buff;
        StackingMode = StackingMode.ReplaceOldWithNew;
        MaxStackCount = 1;
        this.duration = duration;
        Duration = duration;
        this.critIncrease = critIncrease;
    }

    public override void OnApply(Stats targetStats)
    {
        _targetStats = targetStats;
        _aggregatorId = _targetStats.AddCritContribution(critIncrease);
        _isApplied = true;
    }

    public override void OnUpdate(Stats targetStats, float deltaTime)
    {
    }

    public override void OnRemove(Stats targetStats)
    {
        if (_isApplied && _aggregatorId >= 0)
        {
            _targetStats.RemoveCritContribution(_aggregatorId);
            _aggregatorId = -1;
            _isApplied = false;
        }
    }
} 