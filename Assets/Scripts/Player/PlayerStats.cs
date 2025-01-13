using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthRegen = 0f;
    [SerializeField] private float baseDamageMultiplier = 1f;
    [SerializeField] private float baseFireRateMultiplier = 1f;
    [SerializeField] private float critChance = 0.05f;
    [SerializeField] private float critMultiplier = 2f;

    // Field-related stats
    [SerializeField] private float healingFieldsStrengthMultiplier = 1f;
    [SerializeField] private float chargeUpFieldsSpeedMultiplier = 1f;
    [SerializeField] private float decayingChargeUpFieldsDecaySlowdownMultiplier = 1f;
    [SerializeField] private float positiveNegativeFieldsEffectivenessMultiplier = 1f;
    [SerializeField] private float additionalProjectilesForAttacks = 0f; // This is a whole number, not a percentage
    [SerializeField] private float lingeringBuffFieldsDurationMultiplier = 1f;
    [SerializeField] private float lingeringBuffFieldsEffectivenessMultiplier = 1f;
    [SerializeField] private float fieldsCooldownMultiplier = 1f;

    public float MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }
    public float HealthRegen { get { return healthRegen; } private set { healthRegen = value; } }
    public float BaseDamageMultiplier { get { return baseDamageMultiplier; } private set { baseDamageMultiplier = value; } }
    public float BaseFireRateMultiplier { get { return baseFireRateMultiplier; } private set { baseFireRateMultiplier = value; } }
    public float CritChance { get { return critChance; } private set { critChance = value; } }
    public float CritMultiplier { get { return critMultiplier; } private set { critMultiplier = value; } }

    // Field-related properties
    public float HealingFieldsStrengthMultiplier { get { return healingFieldsStrengthMultiplier; } private set { healingFieldsStrengthMultiplier = value; } }
    public float ChargeUpFieldsSpeedMultiplier { get { return chargeUpFieldsSpeedMultiplier; } private set { chargeUpFieldsSpeedMultiplier = value; } }
    public float DecayingChargeUpFieldsDecaySlowdownMultiplier { get { return decayingChargeUpFieldsDecaySlowdownMultiplier; } private set { decayingChargeUpFieldsDecaySlowdownMultiplier = value; } }
    public float PositiveNegativeFieldsEffectivenessMultiplier { get { return positiveNegativeFieldsEffectivenessMultiplier; } private set { positiveNegativeFieldsEffectivenessMultiplier = value; } }
    public float AdditionalProjectilesForAttacks { get { return additionalProjectilesForAttacks; } private set { additionalProjectilesForAttacks = value; } }
    public float LingeringBuffFieldsDurationMultiplier { get { return lingeringBuffFieldsDurationMultiplier; } private set { lingeringBuffFieldsDurationMultiplier = value; } }
    public float LingeringBuffFieldsEffectivenessMultiplier { get { return lingeringBuffFieldsEffectivenessMultiplier; } private set { lingeringBuffFieldsEffectivenessMultiplier = value; } }
    public float FieldsCooldownMultiplier { get { return fieldsCooldownMultiplier; } private set { fieldsCooldownMultiplier = value; } }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Example modification methods:
    public void IncreaseMaxHealth(float amount)
    {
        MaxHealth += amount;

        // Update PlayerHealth's values directly
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.SetMaxHealth(MaxHealth);
        }
    }

    public void SetHealthRegen(float amount)
    {
        HealthRegen = amount;
    }

    public void MultiplyBaseDamageMultiplier(float multiplier)
    {
        BaseDamageMultiplier *= multiplier;
    }

    public void MultiplyBaseFireRateMultiplier(float multiplier)
    {
        BaseFireRateMultiplier *= multiplier;
    }

    public void SetCritChance(float newCritChance)
    {
        CritChance = Mathf.Clamp01(newCritChance); // Ensure it's between 0 and 1
    }

    public void SetCritMultiplier(float newCritMultiplier)
    {
        CritMultiplier = newCritMultiplier;
    }

    // Field-related modification methods (add more as needed)
    public void MultiplyHealingFieldsStrength(float multiplier)
    {
        HealingFieldsStrengthMultiplier *= multiplier;
    }

    public void MultiplyChargeUpFieldsSpeed(float multiplier)
    {
        ChargeUpFieldsSpeedMultiplier *= multiplier;
    }

    public void MultiplyDecayingChargeUpFieldsDecaySlowdown(float multiplier)
    {
        DecayingChargeUpFieldsDecaySlowdownMultiplier *= multiplier;
    }

    public void MultiplyPositiveNegativeFieldsEffectiveness(float multiplier)
    {
        PositiveNegativeFieldsEffectivenessMultiplier *= multiplier;
    }

    public void AddProjectileToAdditionalProjectilesForAttacks(float amount)
    {
        AdditionalProjectilesForAttacks += amount;
    }

    public void MultiplyLingeringBuffFieldsDuration(float multiplier)
    {
        LingeringBuffFieldsDurationMultiplier *= multiplier;
    }

    public void MultiplyLingeringBuffFieldsEffectiveness(float multiplier)
    {
        LingeringBuffFieldsEffectivenessMultiplier *= multiplier;
    }   

    public void MultiplyFieldsCooldownMultiplier(float multiplier)
    {
        FieldsCooldownMultiplier = Mathf.Clamp01(FieldsCooldownMultiplier * multiplier);
    }
} 