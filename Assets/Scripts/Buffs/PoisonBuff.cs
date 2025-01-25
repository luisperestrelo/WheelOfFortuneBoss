using UnityEngine;

public class PoisonBuff : DamageOverTimeBuff
{
    public PoisonBuff(float dmgPerSecond, float duration)
    {
        BuffId = "Poison";
        BuffType = BuffType.Debuff;
        StackingMode = StackingMode.Independent; 
        // or maybe IncrementStack if you prefer a single entry with a "stack count"

        damagePerSecond = dmgPerSecond;
        Duration = duration;
    }

    public override void OnApply(Stats targetStats)
    {
        // Possibly spawn or enable a poison VFX
        Debug.Log("PoisonBuff OnApply: Starting DoT...");
    }

    public override void OnRemove(Stats targetStats)
    {
        // Stop poison visuals/sfx if needed
        Debug.Log("PoisonBuff OnRemove: Stopped DoT.");
    }
}