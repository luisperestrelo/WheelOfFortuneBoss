using UnityEngine;

//This is our OG Damage Buff, now properly using a Buff system
public class DamageBuff : BuffBase
{
    private Stats _targetStats;
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