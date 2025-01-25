using UnityEngine;

public class DamageBuff : BuffBase
{
    private float damageMultiplier;
    private float originalMultiplier;

    public DamageBuff(float baseMultiplier, float duration)
    {
        BuffId = "DamageBuff";
        BuffType = BuffType.Buff; // For UI: a positive buff
        StackingMode = StackingMode.ReplaceOldWithNew; // For example
        Duration = duration;
        damageMultiplier = baseMultiplier;
    }

    public override void OnApply(PlayerStats targetStats)
    {
        // Save the original so we can revert
        originalMultiplier = targetStats.BaseDamageMultiplier;
        // Multiply the existing baseDamage
        targetStats.SetBaseDamageMultiplier(originalMultiplier * damageMultiplier);
    }

    public override void OnUpdate(PlayerStats targetStats, float deltaTime)
    {
        // No tick-based logic for this example
    }

    public override void OnRemove(PlayerStats targetStats)
    {
        // Revert back
        targetStats.SetBaseDamageMultiplier(originalMultiplier);
    }
} 