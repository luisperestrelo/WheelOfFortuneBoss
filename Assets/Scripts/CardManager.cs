using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Dictionary<UpgradeCategory, float> fieldCategoryUpgrades = new Dictionary<UpgradeCategory, float>();
    public Dictionary<FieldType, Dictionary<FieldUpgradeType, float>> fieldSpecificUpgrades = new Dictionary<FieldType, Dictionary<FieldUpgradeType, float>>();

    public void ApplyCard(Card card)
    {
        if (card is FieldCard fieldCard)
        {
            // Add the field to the WheelManager
            FindObjectOfType<WheelManager>().AddField(fieldCard.field);
        }
        else if (card is StatUpgradeCard statUpgradeCard)
        {
            // Apply stat upgrade to the player
            Player player = FindObjectOfType<Player>(); // Or get the player reference in another way
            PlayerCombat playerCombat = player.GetComponent<PlayerCombat>(); // Get PlayerCombat component once
            if (statUpgradeCard.statType == StatType.Health)
            {
                player.GetComponent<PlayerHealth>().IncreaseMaxHealth(statUpgradeCard.statValue);
            }
            else if (statUpgradeCard.statType == StatType.GlobalDamageMultiplier)
            {
                playerCombat.UpdateBaseDamageMultiplier(playerCombat.GetBaseDamageMultiplier() * (1 + statUpgradeCard.statValue)); // Update baseDamageMultiplier directly
            }
        }
        else if (card is FieldCategoryUpgradeCard fieldCategoryUpgradeCard)
        {
            // Store the field category upgrade
            if (!fieldCategoryUpgrades.ContainsKey(fieldCategoryUpgradeCard.upgradeCategory))
            {
                fieldCategoryUpgrades[fieldCategoryUpgradeCard.upgradeCategory] = 0;
            }
            fieldCategoryUpgrades[fieldCategoryUpgradeCard.upgradeCategory] += fieldCategoryUpgradeCard.upgradeValue;
        }
        else if (card is FieldSpecificUpgradeCard fieldSpecificUpgradeCard)
        {
            // Store the field-specific upgrade
            if (!fieldSpecificUpgrades.ContainsKey(fieldSpecificUpgradeCard.targetFieldType))
            {
                fieldSpecificUpgrades[fieldSpecificUpgradeCard.targetFieldType] = new Dictionary<FieldUpgradeType, float>();
            }
            if (!fieldSpecificUpgrades[fieldSpecificUpgradeCard.targetFieldType].ContainsKey(fieldSpecificUpgradeCard.fieldUpgradeType))
            {
                fieldSpecificUpgrades[fieldSpecificUpgradeCard.targetFieldType][fieldSpecificUpgradeCard.fieldUpgradeType] = 0;
            }
            fieldSpecificUpgrades[fieldSpecificUpgradeCard.targetFieldType][fieldSpecificUpgradeCard.fieldUpgradeType] += fieldSpecificUpgradeCard.fieldUpgradeValue;
        }
    }

    // Example of how to use the upgrades in a FieldEffectHandler:
    public float GetChargeSpeedMultiplier(Field field)
    {
        float multiplier = 1f;
        if (fieldCategoryUpgrades.ContainsKey(UpgradeCategory.ChargeSpeed))
        {
            multiplier += fieldCategoryUpgrades[UpgradeCategory.ChargeSpeed];
        }
        // Check for field-specific upgrades
        if (fieldSpecificUpgrades.ContainsKey(field.FieldType))
        {
            if (fieldSpecificUpgrades[field.FieldType].ContainsKey(FieldUpgradeType.ChargeTime))
            {
                multiplier += fieldSpecificUpgrades[field.FieldType][FieldUpgradeType.ChargeTime];
            }
        }

        return multiplier;
    }
} 