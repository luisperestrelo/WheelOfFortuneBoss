using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHelper : MonoBehaviour
{
    public static readonly Dictionary<StatType, StatDisplayData> StatTypeToDisplayDataMapping = new()
    {
        {
            StatType.Health,
            new StatDisplayData
            {
                Name = "Health",
                ToolTipText = "Represents your total health pool. Don't reach 0!",
                Type = StatDisplayType.Absolute
            }
        },
        {
            StatType.HealthRegen,
            new StatDisplayData
            {
                Name = "Health Regeneration",
                ToolTipText = "Passive Health Regeneration.",
                Type = StatDisplayType.TimeRate
            }
        },
        {
            StatType.GlobalDamageMultiplier,
            new StatDisplayData
            {
                Name = "Bonus Damage",
                ToolTipText = "Global bonus damage.",
                Type = StatDisplayType.Percentage

            }
        },
        {
            StatType.BaseFireRateMultiplier,
            new StatDisplayData
            {
                Name = "Base Attacks Per Second",
                ToolTipText = "Your baseline fire rate. Attacks use this stat to calculate their fire rate.",
                Type = StatDisplayType.TimeRate, // TODO: ?
                FlippedSign = true
            }
        },

        {
            StatType.CritChance,
            new StatDisplayData
            {
                Name = "Critical Hit Chance",
                ToolTipText = "The percentage chance to deal a critical hit, causing extra damage.",
                Type = StatDisplayType.Percentage
            }
        },
        {
            StatType.CritMultiplier,
            new StatDisplayData
            {
                Name = "Critical Damage Multiplier",
                ToolTipText = "The multiplier applied to damage when landing a critical hit.",
                Type = StatDisplayType.Multiplier
            }
        },

        // Field-related stats
        {
            StatType.HealingFieldsStrength,
            new StatDisplayData
            {
                Name = "Healing Fields Effectiveness",
                ToolTipText = "The effectiveness of healing fields.",
                Type = StatDisplayType.Percentage
            }
        },
        {
            StatType.ChargeUpFieldsSpeed,
            new StatDisplayData
            {
                Name = "Charge-Up Field Speed",
                ToolTipText = "Multiplier for the speed at which charge-up fields charge-up.",
                Type = StatDisplayType.Percentage // TODO: ? or TimeRate

            }
        },
        {
            StatType.DecayingChargeUpFieldsDecaySlowdown,
            new StatDisplayData
            {
                Name = "Charge-Up Field Decay",
                ToolTipText = "Multiplier for the speed at which charge-up fields decay when not standing on them.",
                Type = StatDisplayType.Percentage // TODO: ?

            }
        },
        {
            StatType.PositiveNegativeFieldsEffectiveness,
            new StatDisplayData
            {
                Name = "Positive/Negative Fields Effectiveness",
                ToolTipText = "Both the Positive and Negative effects of Positive/Negative Fields are multiplied by this stat.",
                Type = StatDisplayType.Percentage
            }
        },
        {
            StatType.LingeringBuffFieldsDuration,
            new StatDisplayData
            {
                Name = "Field Buff Duration Multiplier",
                ToolTipText = "Multiplier for the duration of buffs granted by fields.",
                Type = StatDisplayType.Percentage // TODO: ?
            }

        },
        {
            StatType.LingeringBuffFieldsEffectiveness,
            new StatDisplayData
            {
                Name = "Field Buff Effectiveness",
                ToolTipText = "Multiplier for the strength of buffs provided by fields.",
                Type = StatDisplayType.Percentage
            }

        },
        {
            StatType.FieldsCooldownReduction,
            new StatDisplayData
            {
                Name = "Cooldown Reduction",
                ToolTipText = "Fields that have a cooldown are reduced by this multiplier.",
                Type = StatDisplayType.Percentage // TODO: ?
            }


        },
        {
            StatType.AdditionalProjectilesForAttacks,
            new StatDisplayData
            {
                Name = "Additional Projectiles",
                ToolTipText = "Adds extra projectiles to your attacks.",
                Type = StatDisplayType.Absolute
            }
        },
        {
            StatType.CritsGiveAttackSpeedBuff,
            new StatDisplayData
            {
                Name = "Attack Speed On Crits",
                ToolTipText = "Critical hits provide a temporary boost to attack speed.",  
                Type = StatDisplayType.Percentage, //TODO: ?,
                FlippedSign = true
            }
        },
        {
            StatType.CritsGiveStackingDamageBuff,
            new StatDisplayData
            {
                Name = "Damage Increase On Crits",
                ToolTipText = "Critical hits provide a stacking damage buff.", 
                Type = StatDisplayType.Percentage, //TODO: ?
            }
        },
        {
            StatType.PoisonChance,
            new StatDisplayData
            {
                Name = "Poison Chance",
                ToolTipText = "Chance to apply poison. Base poison deals 1 DPS for 4s, stacking indefinitely.",
                Type = StatDisplayType.Percentage,
            }
        },
        {
            StatType.PoisonDamageOverTimeMultiplier,
            new StatDisplayData
            {
                Name = "Poison Damage",
                ToolTipText = "Multiplier for poison damage. Base poison deals 1 DPS for 4s, stacking indefinitely.",
                Type = StatDisplayType.Multiplier,
            }
        },
        {
            StatType.PoisonDurationMultiplier,
            new StatDisplayData
            {
                Name = "Poison Duration",
                ToolTipText = "Multiplier for poison duration. Base poison deals 1 DPS for 4s, stacking indefinitely.",
                Type = StatDisplayType.Percentage,
            }
        },
        {
            StatType.FullCirclesGiveDamageBuff,
            new StatDisplayData
            {
                Name = "Damage Increase on Full Circles",
                ToolTipText = "A Full Circle provides a stacking damage buff. Resets when you change direction",
                Type = StatDisplayType.Percentage,
            }
        }
    };


    // Showing the total value
    public static string GetStatsDisplayText(StatType statType, PlayerStats playerStats)
    {
        return StatTypeToDisplayDataMapping.TryGetValue(statType, out var data)
            ? $"{FormatValue(GetStatValue(statType, playerStats), data.Type)} {data.Name}"
            : "";
    }

    // Used for temporary stats, showing only the additional stat value
    public static string GetStatsDisplayText(StatType statType, float value)
    {
        if (!StatTypeToDisplayDataMapping.TryGetValue(statType, out var data))
            return "";
        value = data.FlippedSign ? -value : value;
        var sign = value > 0 ? "+" : "-";
        return $"{sign} {FormatValue(Mathf.Abs(value), data.Type)} {data.Name}";

    }

    private static string FormatValue(float value, StatDisplayType displayType)
    {
        if (displayType == StatDisplayType.NoValue) 
            return "";

        if (displayType == StatDisplayType.Percentage)
            value *= 100;


        string valueString = value.ToString("0.##"); // max 2 decimals

        return valueString + GetValueSuffix(displayType);
    }

    public static string GetTooltipText(StatType statType)
    {
        return StatTypeToDisplayDataMapping.TryGetValue(statType, out var statDisplayData)
            ? statDisplayData.ToolTipText
            : "";
    }

    public static float GetStatValue(StatType statType, PlayerStats playerStats)
    {
        return statType switch
        {
            // Base Stats
            StatType.Health => playerStats.MaxHealth,
            StatType.HealthRegen => playerStats.HealthRegen,
            StatType.GlobalDamageMultiplier => (playerStats.BaseDamageMultiplier - 1),
            StatType.BaseFireRateMultiplier => 1/playerStats.BaseFireRateMultiplier,
            StatType.CritChance => playerStats.CritChance,
            StatType.CritMultiplier => playerStats.CritMultiplier,

            // Field-Related Stats
            StatType.HealingFieldsStrength => playerStats.HealingFieldsStrengthMultiplier,
            StatType.ChargeUpFieldsSpeed => playerStats.ChargeUpFieldsSpeedMultiplier,
            StatType.DecayingChargeUpFieldsDecaySlowdown => playerStats.DecayingChargeUpFieldsDecaySlowdownMultiplier,
            StatType.PositiveNegativeFieldsEffectiveness => playerStats.PositiveNegativeFieldsEffectivenessMultiplier,
            StatType.AdditionalProjectilesForAttacks => playerStats.AdditionalProjectilesForAttacks,
            StatType.LingeringBuffFieldsDuration => playerStats.LingeringBuffFieldsDurationMultiplier,
            StatType.LingeringBuffFieldsEffectiveness => playerStats.LingeringBuffFieldsEffectivenessMultiplier,
            StatType.FieldsCooldownReduction => (1 - playerStats.FieldsCooldownMultiplier),
            

            // Specialized Stats
            StatType.PoisonChance => playerStats.PoisonChance,
            StatType.PoisonDamageOverTimeMultiplier => playerStats.PoisonDamageOverTimeMultiplier,
            StatType.PoisonDurationMultiplier => playerStats.PoisonDurationMultiplier,

            StatType.CritsGiveAttackSpeedBuff => 0.25f, //TODO
            StatType.CritsGiveStackingDamageBuff => 0.10f,
            StatType.FullCirclesGiveDamageBuff => 0.20f,



            // Default case for unmapped stats
            _ => 0f
        };
    }

    public static string GetValueSuffix(StatDisplayType type)
    {
        return type switch
        {
            StatDisplayType.Multiplier => "x",
            StatDisplayType.Percentage => "%",
            StatDisplayType.TimeRate => "/s",
            StatDisplayType.Duration => "s",
            _ => ""
        };
    }

    public static Color GetRarityColor(CardRarity rarity)
    {
        return rarity switch
        {
            CardRarity.Common => new Color(170f / 255f, 157 / 255f, 170 / 255f),
            CardRarity.Rare => new Color(28f / 255f, 96f / 255f, 206f / 255f),
            CardRarity.Epic => new Color(206f / 255f, 28f / 255f, 196f / 255f),
            _ => Color.white
        };
    }
}