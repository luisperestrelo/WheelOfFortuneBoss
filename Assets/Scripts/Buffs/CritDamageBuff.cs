using UnityEngine;

public class CritDamageBuff : BuffBase
{
    private Stats _targetStats;
    private bool _isApplied = false;
    private int _aggregatorId = -1;

    private float damageMultiplier = 1.1f; // +10%
    private float duration;

    public CritDamageBuff(float damageMultiplier, float duration)
    {
        BuffId = "CritDamageBuff";
        BuffType = BuffType.Buff;
        StackingMode = StackingMode.Independent; 
        MaxStackCount = 10;  
        this.duration = duration;
        Duration = duration;
        this.damageMultiplier = damageMultiplier;
    }

    public override void OnApply(Stats targetStats)
    {
        _targetStats = targetStats;
        _aggregatorId = _targetStats.AddDamageContribution(damageMultiplier);
        _isApplied = true;
    }

    public override void OnUpdate(Stats targetStats, float deltaTime)
    {
    }

    public override void OnRemove(Stats targetStats)
    {
        if (_isApplied && _aggregatorId >= 0)
        {
            _targetStats.RemoveDamageContribution(_aggregatorId);
            _aggregatorId = -1;
            _isApplied = false;
        }
    }
} 