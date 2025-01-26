using UnityEngine;

public class CritAttackSpeedBuff : BuffBase
{
    private Stats _targetStats;
    private bool _isApplied = false;

    private float attackSpeedMultiplier = 1.1f; // +10%
    private float duration;

    public CritAttackSpeedBuff(float attackSpeedMultiplier, float duration)
    {
        BuffId = "CritAttackSpeedBuff";
        BuffType = BuffType.Buff;
        StackingMode = StackingMode.ReplaceOldWithNew;
        MaxStackCount = 1;
        this.duration = duration;
        Duration = duration;
        this.attackSpeedMultiplier = attackSpeedMultiplier;
    }

    public override void OnApply(Stats targetStats)
    {
        _targetStats = targetStats;
        _targetStats.MultiplyBaseFireRateMultiplier(attackSpeedMultiplier);
        _isApplied = true;
    }

    public override void OnUpdate(Stats targetStats, float deltaTime)
    {
        // No per-frame logic needed
    }

    public override void OnRemove(Stats targetStats)
    {
        if (_isApplied)
        {
            // Revert the effect
            float revertFactor = 1f / attackSpeedMultiplier;
            _targetStats.MultiplyBaseFireRateMultiplier(revertFactor);
            _isApplied = false;
        }
    }
}