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
        PlayerCombat playerCombat = player.GetComponent<PlayerCombat>();
        Debug.Log("Entering Damage Buff Field, boosting damage by " + damageMultiplier);

        // Apply the positive-negative field effectiveness multiplier to the damage multiplier
        float finalMultiplier = damageMultiplier * playerStats.PositiveNegativeFieldsEffectivenessMultiplier; //TODO change to buff fields

        // Apply the lingering buff duration multiplier from PlayerStats
        float finalDuration = duration * playerStats.LingeringBuffFieldsDurationMultiplier;

        playerCombat.SetDamageMultiplierForDuration(finalMultiplier, finalDuration);
    }

    public override void OnStay(Player player, float deltaTime)
    {
        PlayerCombat playerCombat = player.GetComponent<PlayerCombat>();

        // Apply the positive-negative field effectiveness multiplier to the damage multiplier
        float finalMultiplier = damageMultiplier * playerStats.PositiveNegativeFieldsEffectivenessMultiplier;

        // Apply the lingering buff duration multiplier from PlayerStats
        float finalDuration = duration * playerStats.LingeringBuffFieldsDurationMultiplier;

        playerCombat.SetDamageMultiplierForDuration(finalMultiplier, finalDuration); //it constantly refreshes
    }

    public override void OnExit(Player player)
    {
        PlayerCombat playerCombat = player.GetComponent<PlayerCombat>();

        // Apply the positive-negative field effectiveness multiplier to the damage multiplier
        float finalMultiplier = damageMultiplier * playerStats.PositiveNegativeFieldsEffectivenessMultiplier;

        // Apply the lingering buff duration multiplier from PlayerStats
        float finalDuration = duration * playerStats.LingeringBuffFieldsDurationMultiplier;

        playerCombat.SetDamageMultiplierForDuration(finalMultiplier, finalDuration);
        Debug.Log("Exiting Damage Buff Field, boosting damage by " + finalMultiplier + " for " + finalDuration + " seconds ");
        // The damage multiplier is automatically reset after the duration in PlayerCombat
    }
}