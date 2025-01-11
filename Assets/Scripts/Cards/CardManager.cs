using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
   

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

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
            if (statUpgradeCard.statType == StatType.Health)
            {
                playerStats.IncreaseMaxHealth(statUpgradeCard.statValue);
            }
            else if (statUpgradeCard.statType == StatType.GlobalDamageMultiplier)
            {
                playerStats.MultiplyBaseDamage(1 + statUpgradeCard.statValue);
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