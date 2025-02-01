using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private CritUpgrades critUpgrades;
    private CardPool cardPool;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        critUpgrades = FindObjectOfType<CritUpgrades>();   
        cardPool = FindObjectOfType<CardPool>();
    }

    public void ApplyCard(Card card)
    {
        if (card is FieldCard fieldCard)
        {
            RunManager.Instance.wheelManager.AddField(fieldCard.field);
        }
        else if (card is StatUpgradeCard statUpgradeCard)
        {
            // Iterate through the lists of statTypes and statValues. These are always a pair. Some cards can affect multiple stats
            for (int i = 0; i < statUpgradeCard.statTypes.Count; i++)
            {
                StatType statType = statUpgradeCard.statTypes[i];
                float statValue = statUpgradeCard.statValues[i];

                if (statType == StatType.Health)
                {
                    playerStats.IncreaseMaxHealth(statValue);
                }
                else if (statType == StatType.GlobalDamageMultiplier)
                {
                    playerStats.MultiplyBaseDamageMultiplier(1 + statValue);
                }
                else if (statType == StatType.BaseFireRateMultiplier)
                {
                    playerStats.MultiplyBaseFireRateMultiplier(1 + statValue);
                }
                else if (statType == StatType.CritChance)
                {
                    playerStats.SetCritChance(playerStats.CritChance + statValue);
                }
                else if (statType == StatType.CritMultiplier)
                {
                    playerStats.SetCritMultiplier(playerStats.CritMultiplier + statValue);
                }
                else if (statType == StatType.HealthRegen)
                {
                    playerStats.SetHealthRegen(playerStats.HealthRegen + statValue);
                }
                // Field-related upgrades
                else if (statType == StatType.HealingFieldsStrength)
                {
                    playerStats.MultiplyHealingFieldsStrength(1 + statValue);
                }
                else if (statType == StatType.ChargeUpFieldsSpeed)
                {
                    playerStats.MultiplyChargeUpFieldsSpeed(1 + statValue);
                }
                else if (statType == StatType.DecayingChargeUpFieldsDecaySlowdown)
                {
                    playerStats.MultiplyDecayingChargeUpFieldsDecaySlowdown(1 + statValue);
                }
                else if (statType == StatType.PositiveNegativeFieldsEffectiveness)
                {
                    playerStats.MultiplyPositiveNegativeFieldsEffectiveness(1 + statValue);
                }
                else if (statType == StatType.AdditionalProjectilesForAttacks)
                {
                    playerStats.AddProjectileToAdditionalProjectilesForAttacks(statValue);
                }
                else if (statType == StatType.LingeringBuffFieldsDuration)
                {
                    playerStats.MultiplyLingeringBuffFieldsDuration(1 + statValue);
                }
                else if (statType == StatType.LingeringBuffFieldsEffectiveness)
                {
                    playerStats.MultiplyLingeringBuffFieldsEffectiveness(1 + statValue);
                }
                else if (statType == StatType.FieldsCooldownReduction)
                {
                    playerStats.MultiplyFieldsCooldownMultiplier(1 - statValue);
                }
                else if (statType == StatType.CritsGiveAttackSpeedBuff)
                {
                    critUpgrades.AddCritEffect(new CritAttackSpeedBuffEffect(1 + statValue, 2f));
                    cardPool.RemoveAllCardsOfStatType(StatType.CritsGiveAttackSpeedBuff); // Remove the card from the pool
                }
                else if (statType == StatType.CritsGiveStackingDamageBuff)
                {
                    critUpgrades.AddCritEffect(new CritDamageBuffEffect(1 + statValue, 2f)); 
                    cardPool.RemoveAllCardsOfStatType(StatType.CritsGiveStackingDamageBuff); // Remove the card from the pool
                }
                else if (statType == StatType.PoisonChance)
                {
                    playerStats.SetPoisonChance(playerStats.PoisonChance + statValue);
                }
                else if (statType == StatType.PoisonDamageOverTimeMultiplier)
                {
                    playerStats.MultiplyPoisonDamageOverTimeMultiplier(1 + statValue);
                }
                else if (statType == StatType.PoisonDurationMultiplier)
                {
                    playerStats.MultiplyPoisonDurationMultiplier(1 + statValue);
                }
                else if (statType == StatType.FullCirclesGiveDamageBuff)
                {
                    playerStats.SetHasFullCircleBuffUpgrade(true);
                    cardPool.RemoveAllCardsOfStatType(StatType.FullCirclesGiveDamageBuff); // Right now it is Epic so it shouldnt be needed,
                                                                                           // but if we change this ,it's already done
                }

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