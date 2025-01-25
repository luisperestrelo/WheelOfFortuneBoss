using UnityEngine;

//TODO will be cleaner when we have a BuffManager script
public class DamageBuffEffectHandler : FieldEffectHandler
{
    private float damageMultiplier;
    private float duration;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        damageMultiplier = ((DamageBuffField)fieldData).DamageMultiplier;
        duration = fieldData.Duration;
    }

    public override void OnEnter(Player player)
    {
        var playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            var buffManager = playerStats.GetComponent<BuffManager>();
            if (buffManager != null)
            {
                float finalMultiplier = damageMultiplier * playerStats.LingeringBuffFieldsEffectivenessMultiplier;
                float finalDuration = duration * playerStats.LingeringBuffFieldsDurationMultiplier;
                DamageBuff dmgBuff = new DamageBuff(finalMultiplier, finalDuration);
                buffManager.ApplyBuff(dmgBuff);
                Debug.Log("Entering Damage Buff Field with multiplier=" + finalMultiplier + " for " + finalDuration + "s");
            }
        }
    }

    public override void OnStay(Player player, float deltaTime)
    {
        var playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            var buffManager = playerStats.GetComponent<BuffManager>();
            if (buffManager != null)
            {
                float finalMultiplier = damageMultiplier * playerStats.LingeringBuffFieldsEffectivenessMultiplier;
                float finalDuration = duration * playerStats.LingeringBuffFieldsDurationMultiplier;
                DamageBuff dmgBuff = new DamageBuff(finalMultiplier, finalDuration);
                buffManager.ApplyBuff(dmgBuff);
            }
        }
        // Debug or logs as needed...
    }

    public override void OnExit(Player player)
    {
        // Currently, we do nothing special on exit. We can let the buff linger
        // for its remaining duration. If you want it removed immediately:
        // var playerStats = player.GetComponent<PlayerStats>();
        // var buffManager = playerStats.GetComponent<BuffManager>();
        // buffManager.RemoveBuffImmediately("DamageBuff");
        Debug.Log("Exiting Damage Buff Field");
    }
}