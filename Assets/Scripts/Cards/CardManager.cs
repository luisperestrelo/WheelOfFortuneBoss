using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public void ApplyCard(Card card)
    {
        if (card is FieldCard fieldCard)
        {
            RunManager.Instance.wheelManager.AddField(fieldCard.field);
        }
        else if (card is StatUpgradeCard statUpgradeCard)
        {
            // Apply stat upgrade to the player
            if (statUpgradeCard.statType == StatType.Health)
            {
                playerStats.IncreaseMaxHealth(statUpgradeCard.statValue);
            }
            else if (statUpgradeCard.statType == StatType.GlobalDamageMultiplier)
            {
                playerStats.MultiplyBaseDamageMultiplier(1 + statUpgradeCard.statValue);
            }
            else if (statUpgradeCard.statType == StatType.BaseFireRateMultiplier)
            {
                playerStats.MultiplyBaseFireRateMultiplier(1 + statUpgradeCard.statValue);
            }
            else if (statUpgradeCard.statType == StatType.CritChance)
            {
                playerStats.SetCritChance(playerStats.CritChance + statUpgradeCard.statValue);
            }
            else if (statUpgradeCard.statType == StatType.CritMultiplier)
            {
                playerStats.SetCritMultiplier(playerStats.CritMultiplier + statUpgradeCard.statValue);
            }
            else if (statUpgradeCard.statType == StatType.HealthRegen)
            {
                playerStats.SetHealthRegen(playerStats.HealthRegen + statUpgradeCard.statValue);
            }
            // Field-related upgrades
            else if (statUpgradeCard.statType == StatType.HealingFieldsStrength)
            {
                playerStats.MultiplyHealingFieldsStrength(1 + statUpgradeCard.statValue);
            }
            else if (statUpgradeCard.statType == StatType.ChargeUpFieldsSpeed)
            {
                playerStats.MultiplyChargeUpFieldsSpeed(1 + statUpgradeCard.statValue);
            }
            else if (statUpgradeCard.statType == StatType.DecayingChargeUpFieldsDecaySlowdown)
            {
                playerStats.MultiplyDecayingChargeUpFieldsDecaySlowdown(1 + statUpgradeCard.statValue);
            }
            else if (statUpgradeCard.statType == StatType.PositiveNegativeFieldsEffectiveness)
            {
                playerStats.MultiplyPositiveNegativeFieldsEffectiveness(1 + statUpgradeCard.statValue);
            }
/*             else if (statUpgradeCard.statType == StatType.ProjectileReplacingFieldsAdditionalProjectiles)
            {
                playerStats.AddProjectileToProjectileReplacingFields(statUpgradeCard.statValue); // Add (not multiply) projectiles
            } */
            else if (statUpgradeCard.statType == StatType.LingeringBuffFieldsDuration)
            {
                playerStats.MultiplyLingeringBuffFieldsDuration(1 + statUpgradeCard.statValue);
            }

        }
        else if (card is FieldCategoryUpgradeCard fieldCategoryUpgradeCard)
        {

        }
        else if (card is FieldSpecificUpgradeCard fieldSpecificUpgradeCard)
        {

        }

        //else if // thinking of adding cards that interact with the wheel like increasing the size of a field
    }
}