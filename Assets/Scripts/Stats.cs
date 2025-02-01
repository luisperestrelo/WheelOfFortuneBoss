using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthRegen = 0f;
    [SerializeField] private float baseDamageMultiplier = 1f;
    [SerializeField] private float baseFireRateMultiplier = 1f;
    [SerializeField] private float critChance = 0.05f;
    [SerializeField] private float critMultiplier = 2f;
    [SerializeField] private float baseDamageTakenMultiplier = 1f;

    // Field-related stats
    [SerializeField] private float healingFieldsStrengthMultiplier = 1f;
    [SerializeField] private float chargeUpFieldsSpeedMultiplier = 1f;
    [SerializeField] private float decayingChargeUpFieldsDecaySlowdownMultiplier = 1f;
    [SerializeField] private float positiveNegativeFieldsEffectivenessMultiplier = 1f;
    [SerializeField] private float additionalProjectilesForAttacks = 0f; // This is a whole number, not a percentage
    [SerializeField] private float lingeringBuffFieldsDurationMultiplier = 1f;
    [SerializeField] private float lingeringBuffFieldsEffectivenessMultiplier = 1f;
    [SerializeField] private float fieldsCooldownMultiplier = 1f;

    //More specialized stats
    [SerializeField] private float damageOverTimeMultiplier = 1f;
    [SerializeField] private float poisonDamageOverTimeMultiplier = 1f;
    [SerializeField] private float poisonDurationMultiplier = 1f;
    [SerializeField] private float poisonChance = 0f;
    [SerializeField] private float basePoisonDamage = 1f;
    [SerializeField] private float basePoisonDuration = 4f;

    //Other
    [SerializeField] private bool hasFullCircleBuffUpgrade = false;

    public float MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }
    public float HealthRegen { get { return healthRegen; } private set { healthRegen = value; } }
    public float BaseDamageMultiplier { get { return baseDamageMultiplier; } private set { baseDamageMultiplier = value; } }
    public float BaseFireRateMultiplier { get { return baseFireRateMultiplier; } private set { baseFireRateMultiplier = value; } }
    public float CritChance { get { return critChance; } private set { critChance = value; } }
    public float CritMultiplier { get { return critMultiplier; } private set { critMultiplier = value; } }
    public float BaseDamageTakenMultiplier { get { return baseDamageTakenMultiplier; } private set { baseDamageTakenMultiplier = value; } }

    // Field-related properties
    public float HealingFieldsStrengthMultiplier { get { return healingFieldsStrengthMultiplier; } private set { healingFieldsStrengthMultiplier = value; } }
    public float ChargeUpFieldsSpeedMultiplier { get { return chargeUpFieldsSpeedMultiplier; } private set { chargeUpFieldsSpeedMultiplier = value; } }
    public float DecayingChargeUpFieldsDecaySlowdownMultiplier { get { return decayingChargeUpFieldsDecaySlowdownMultiplier; } private set { decayingChargeUpFieldsDecaySlowdownMultiplier = value; } }
    public float PositiveNegativeFieldsEffectivenessMultiplier { get { return positiveNegativeFieldsEffectivenessMultiplier; } private set { positiveNegativeFieldsEffectivenessMultiplier = value; } }
    public float AdditionalProjectilesForAttacks { get { return additionalProjectilesForAttacks; } private set { additionalProjectilesForAttacks = value; } }
    public float LingeringBuffFieldsDurationMultiplier { get { return lingeringBuffFieldsDurationMultiplier; } private set { lingeringBuffFieldsDurationMultiplier = value; } }
    public float LingeringBuffFieldsEffectivenessMultiplier { get { return lingeringBuffFieldsEffectivenessMultiplier; } private set { lingeringBuffFieldsEffectivenessMultiplier = value; } }
    public float FieldsCooldownMultiplier { get { return fieldsCooldownMultiplier; } private set { fieldsCooldownMultiplier = value; } }

    // More specialized stats
    public float DamageOverTimeMultiplier { get { return damageOverTimeMultiplier; } private set { damageOverTimeMultiplier = value; } }
    public float PoisonDamageOverTimeMultiplier { get { return poisonDamageOverTimeMultiplier; } private set { poisonDamageOverTimeMultiplier = value; } }
    public float PoisonDurationMultiplier { get { return poisonDurationMultiplier; } private set { poisonDurationMultiplier = value; } }
    public float PoisonChance { get { return poisonChance; } private set { poisonChance = value; } }
    public float BasePoisonDamage { get { return basePoisonDamage; } private set { basePoisonDamage = value; } }
    public float BasePoisonDuration { get { return basePoisonDuration; } private set { basePoisonDuration = value; } }

    // Other
    public bool HasFullCircleBuffUpgrade { get { return hasFullCircleBuffUpgrade; } private set { hasFullCircleBuffUpgrade = value; } }

    // Stat-Aggregators
    // Damage aggregator for stacking damage buffs (multiplicative)
    private Dictionary<int, float> activeDamageContributions = new Dictionary<int, float>();
    private int nextDamageId = 0;

    // Crit aggregator for additive crit buffs (additive)
    private Dictionary<int, float> activeCritContributions = new Dictionary<int, float>();
    private int nextCritId = 0;

    // Damage-taken aggregator for stacking damage taken buffs (multiplicative)
    private Dictionary<int, float> activeDamageTakenContributions = new Dictionary<int, float>();
    private int nextDamageTakenId = 0;

    // just to see in the inspector
    [Space]
    [Header("Stats with temporary buffs")]
    [SerializeField] private float aggregatedCritChance = 0f;
    [SerializeField] private float aggregatedDamageMultiplier = 0f;
    [SerializeField] private float aggregatedDamageTakenMultiplier = 0f;


    public int AddDamageContribution(float multiplier)
    {
        int id = nextDamageId++;
        activeDamageContributions[id] = multiplier; // each buff can have e.g. 1.07 for +7%
        return id;
    }

    public void UpdateDamageContribution(int id, float newValue)
    {
        if (activeDamageContributions.ContainsKey(id))
        {
            activeDamageContributions[id] = newValue;
        }
    }

    public void RemoveDamageContribution(int id)
    {
        if (activeDamageContributions.ContainsKey(id))
        {
            activeDamageContributions.Remove(id);
        }
    }

    /// <summary>
    /// Multiply all active damage contributions together.
    /// Then multiply by baseDamageMultiplier (if desired).
    /// Example: If we have three instances each with 1.07 => final = 1.07 * 1.07 * 1.07 ...
    /// </summary>
    public float GetAggregatedDamageMultiplier()
    {
        float total = 1f;
        foreach (var val in activeDamageContributions.Values)
        {
            total *= val;
        }
        return baseDamageMultiplier * total;
    }

    // CritChance (additive)
    public int AddCritContribution(float extraCrit)
    {
        int id = nextCritId++;
        activeCritContributions[id] = extraCrit; // e.g. +1.0 => +100%
        return id;
    }

    public void UpdateCritContribution(int id, float newValue)
    {
        if (activeCritContributions.ContainsKey(id))
        {
            activeCritContributions[id] = newValue;
        }
    }

    public void RemoveCritContribution(int id)
    {
        if (activeCritContributions.ContainsKey(id))
        {
            activeCritContributions.Remove(id);
        }
    }

    /// <summary>
    /// Sum up all active crit contributions, add to base, clamp to 1.0
    /// e.g. base=0.05 => 5%. If we have +1.0 => total = 1.05 => 105% => clamp 1.0
    /// </summary>
    public float GetAggregatedCritChance()
    {
        float sum = critChance;
        foreach (var val in activeCritContributions.Values)
        {
            sum += val;
        }
        return Mathf.Clamp01(sum);
    }

    // Damage-taken aggregator
    public int AddDamageTakenContribution(float multiplier)
    {
        int id = nextDamageTakenId++;
        activeDamageTakenContributions[id] = multiplier; // e.g. 1.20 => +20% 
        return id;
    }

    public void RemoveDamageTakenContribution(int id)
    {
        if (activeDamageTakenContributions.ContainsKey(id))
        {
            activeDamageTakenContributions.Remove(id);
        }
    }

    /// <summary>
    /// Multiply all active damage-taken contributions together,
    /// then multiply by baseDamageTakenMultiplier. 
    /// Example: if we have two 1.2 multipliers => final = baseDamageTakenMultiplier * 1.2 * 1.2
    /// </summary>
    public float GetAggregatedDamageTakenMultiplier()
    {
        float total = 1f;
        foreach (var val in activeDamageTakenContributions.Values)
        {
            total *= val;
        }
        //Debug.Log("Total damage taken multiplier: " + total);
        return baseDamageTakenMultiplier * total;
    }

    public void IncreaseMaxHealth(float amount)
    {
        MaxHealth += amount;

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

    public void SetBaseDamageMultiplier(float multiplier)
    {
        BaseDamageMultiplier = multiplier;
    }

    public void MultiplyBaseDamageMultiplier(float multiplier)
    {
        BaseDamageMultiplier *= multiplier;
    }

    public void MultiplyBaseFireRateMultiplier(float multiplier)
    {
        BaseFireRateMultiplier *= multiplier;
        Debug.Log("BaseFireRateMultiplier: " + BaseFireRateMultiplier);
    }

    public void SetCritChance(float newCritChance)
    {
        CritChance = Mathf.Clamp01(newCritChance); // Ensure it's between 0 and 1
    }

    public void SetCritMultiplier(float newCritMultiplier)
    {
        CritMultiplier = newCritMultiplier;
    }

    public void SetBaseDamageTakenMultiplier(float multiplier)
    {
        BaseDamageTakenMultiplier = multiplier;
    }

    public void MultiplyBaseDamageTakenMultiplier(float multiplier)
    {
        BaseDamageTakenMultiplier *= multiplier;
    }



    // Field-related modification methods 
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

    public void SetPoisonChance(float newPoisonChance)
    {
        PoisonChance = newPoisonChance;
    }

    public void MultiplyPoisonDamageOverTimeMultiplier(float multiplier)
    {
        PoisonDamageOverTimeMultiplier *= multiplier;
    }

    public void MultiplyPoisonDurationMultiplier(float multiplier)
    {
        PoisonDurationMultiplier *= multiplier;
    }

    public void SetHasFullCircleBuffUpgrade(bool value)
    {
        HasFullCircleBuffUpgrade = value;
    }




    private void Update()
    {


#if UNITY_EDITOR

        aggregatedCritChance = GetAggregatedCritChance();
        aggregatedDamageMultiplier = GetAggregatedDamageMultiplier();
        aggregatedDamageTakenMultiplier = GetAggregatedDamageTakenMultiplier();

#endif
    }
}