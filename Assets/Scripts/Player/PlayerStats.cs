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

    // Stat-Aggregators
    // Damage aggregator for stacking damage buffs (multiplicative)
    private Dictionary<int, float> activeDamageContributions = new Dictionary<int, float>();
    private int nextDamageId = 0;

    // Crit aggregator for additive crit buffs (additive)
    private Dictionary<int, float> activeCritContributions = new Dictionary<int, float>();
    private int nextCritId = 0;

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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
    }

    public void SetCritChance(float newCritChance)
    {
        CritChance = Mathf.Clamp01(newCritChance); // Ensure it's between 0 and 1
    }

    public void SetCritMultiplier(float newCritMultiplier)
    {
        CritMultiplier = newCritMultiplier;
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

    private void Update()
    {


#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N))
        {
            BuffManager manager = GetComponent<BuffManager>();
            if (manager != null)
            {
                // e.g. 5 DPS for 3 seconds
                PoisonBuff newPoison = new PoisonBuff(5f, 3f);
                manager.ApplyBuff(newPoison);
                Debug.Log("Applied a PoisonBuff via keyboard 'N'.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BuffManager manager = GetComponent<BuffManager>();
            int poisonStacks = manager.GetTotalStacksOf("Poison");
            if (manager != null)
            {
                int extraDamage = poisonStacks * 5;
                Debug.Log("Poison stacks: " + poisonStacks);
                PlayerHealth playerHealth = GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(extraDamage);
                }
                manager.RemoveBuffImmediately("Poison");
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EnemyBuffManager manager = FindObjectOfType<Boss>().GetComponent<EnemyBuffManager>();
            if (manager != null)
            {
                // e.g. 5 DPS for 3 seconds
                PoisonBuff newPoison = new PoisonBuff(5f, 3f);
                manager.ApplyBuff(newPoison);
                Debug.Log("Applied a PoisonBuff via keyboard '2'.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EnemyBuffManager manager = FindObjectOfType<Boss>().GetComponent<EnemyBuffManager>();
            int poisonStacks = manager.GetTotalStacksOf("Poison");
            if (manager != null)
            {
                int extraDamage = poisonStacks * 5;
                Debug.Log("Poison stacks: " + poisonStacks);
                Health health = FindObjectOfType<Boss>().GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(extraDamage);
                }
                manager.RemoveBuffImmediately("Poison");
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            var buffManager = GetComponent<BuffManager>();
            if (buffManager != null)
            {
                // 7% for e.g. 5 seconds
                var stackingBuff = new StackingDamageBuff(1.07f, 5f);
                buffManager.ApplyBuff(stackingBuff);
                Debug.Log("Applied StackingDamageBuff (7% x multiple stacks) via M key.");
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            var buffManager = GetComponent<BuffManager>();
            if (buffManager != null)
            {
                // +100% crit for 1 second
                var critBuff = new CritBuff(1.0f, 1f);
                buffManager.ApplyBuff(critBuff);
                Debug.Log("Applied CritBuff +100% for 1 second via K key.");
            }
        }


#endif
    }
}