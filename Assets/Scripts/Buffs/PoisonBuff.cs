using UnityEngine;

public class PoisonBuff : DamageOverTimeBuff
{
    public PoisonBuff(float dmgPerSecond, float duration)
    {
        BuffId = "Poison";
        BuffType = BuffType.Debuff;
        StackingMode = StackingMode.Independent; 

        damagePerSecond = dmgPerSecond;
        Duration = duration;
    }

    public override void OnApply(Stats targetStats)
    {
        //Debug.Log("PoisonBuff OnApply: Starting DoT...");
    }

    public override void OnRemove(Stats targetStats)
    {
       // Debug.Log("PoisonBuff OnRemove: Stopped DoT.");
    }
}