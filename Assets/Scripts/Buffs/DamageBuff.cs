using UnityEngine;

public class DamageBuff : BuffBase
{
    private PlayerStats _targetStats;
    private bool isApplied = false;
    private int aggregatorId = -1;

    private float damageMultiplier;

    public DamageBuff(float baseMultiplier, float duration)
    {
        BuffId = "DamageBuff";
        BuffType = BuffType.Buff; 
        StackingMode = StackingMode.ReplaceOldWithNew; 
        Duration = duration;
        damageMultiplier = baseMultiplier;
    }

    public override void OnApply(PlayerStats targetStats)
    {
        _targetStats = targetStats;
        aggregatorId = _targetStats.AddDamageContribution(damageMultiplier);
        isApplied = true;
    }

    public override void OnUpdate(Health targetHealth, float deltaTime)
    {
        // No ticking logic needed
    }

    public override void OnRemove(PlayerStats targetStats)
    {
        if (isApplied && aggregatorId >= 0)
        {
            _targetStats.RemoveDamageContribution(aggregatorId);
            aggregatorId = -1;
            isApplied = false;
        }
    }
} 