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
        Debug.Log("Entering Damage Buff Field, boosting damage by " + damageMultiplier);
        player.GetComponent<PlayerCombat>().SetDamageMultiplierForDuration(damageMultiplier, duration);
    }

    public override void OnStay(Player player, float deltaTime)
    {
        player.GetComponent<PlayerCombat>().SetDamageMultiplierForDuration(damageMultiplier, duration);

    }

    public override void OnExit(Player player)
    {
        player.GetComponent<PlayerCombat>().SetDamageMultiplierForDuration(damageMultiplier, duration);
        Debug.Log("Exiting Damage Buff Field, boosting damage by " + damageMultiplier + " for " + duration + " seconds ");
        // The damage multiplier is automatically reset after the duration in PlayerCombat
    }
}