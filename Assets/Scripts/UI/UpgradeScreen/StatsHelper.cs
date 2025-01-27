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
                ToolTipText = "Represents your total health pool.",
                Type = StatDisplayType.Absolute
            }
        },
        {
            StatType.HealthRegen,
            new StatDisplayData
            {
                Name = "Health Regeneration",
                ToolTipText = "Determines how quickly your health regenerates while in a Healing Field time.",
                Type = StatDisplayType.Time
            }
        },
        {
            StatType.GlobalDamageMultiplier,
            new StatDisplayData
            {
                Name = "Damage Multiplier",
                ToolTipText = "Increases all damage dealt by a percentage.",
                Type = StatDisplayType.Multiplier
            }
        },
        {
            StatType.BaseFireRateMultiplier,
            new StatDisplayData
            {
                Name = "Attack Speed",
                ToolTipText = "Increases the speed at which you attack or fire weapons.",
                Type = StatDisplayType.Multiplier
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
                Name = "Healing Fields Strength",
                ToolTipText = "Increases the effectiveness of healing fields.",
                Type = StatDisplayType.Multiplier
            }
        },
        {
            StatType.ChargeUpFieldsSpeed,
            new StatDisplayData
            {
                Name = "Charge-Up Fields Speed",
                ToolTipText = "Improves the speed at which charge-up fields take effect.",
                Type = StatDisplayType.Multiplier
            }
        },
        {
            StatType.DecayingChargeUpFieldsDecaySlowdown,
            new StatDisplayData
            {
                Name = "Charge-Up Field Decay Slowdown",
                ToolTipText = "Reduces the rate at which charge-up fields lose their effect.",
                Type = StatDisplayType.Percentage
            }
        },
        {
            StatType.PositiveNegativeFieldsEffectiveness,
            new StatDisplayData
            {
                Name = "Positive/Negative Fields Effectiveness",
                ToolTipText = "Increases the effectiveness of both positive and negative fields.",
                Type = StatDisplayType.Multiplier
            }
        },
        {
            StatType.LingeringBuffFieldsDuration,
            new StatDisplayData
            {
                Name = "Lingering Buff Duration",
                ToolTipText = "Extends the duration of buffs provided by fields.",
                Type = StatDisplayType.Time
            }
        },
        {
            StatType.LingeringBuffFieldsEffectiveness,
            new StatDisplayData
            {
                Name = "Lingering Buff Effectiveness",
                ToolTipText = "Improves the strength of buffs provided by lingering fields.",
                Type = StatDisplayType.Multiplier
            }
        },
        {
            StatType.FieldsCooldownReduction,
            new StatDisplayData
            {
                Name = "Field Cooldown Reduction",
                ToolTipText = "Reduces the cooldown time for deploying fields.",
                Type = StatDisplayType.Percentage
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
                Name = "Critical Hits Grant Attack Speed Buff",
                ToolTipText = "Critical hits provide a temporary boost to attack speed.",
                Type = StatDisplayType.Percentage,
            }
        },
        {
            StatType.CritsGiveStackingDamageBuff,
            new StatDisplayData
            {
                Name = "Critical Hits Grant Stacking Damage Buff",
                ToolTipText = "Critical hits provide a stacking damage buff.",
                Type = StatDisplayType.Multiplier,
            }
        },
        {
            StatType.PoisonChance,
            new StatDisplayData
            {
                Name = "Poison Chance",
                ToolTipText = "Determines the probability of applying poison effects on hit.",
                Type = StatDisplayType.Percentage,
            }
        },
        {
            StatType.PoisonDamageOverTimeMultiplier,
            new StatDisplayData
            {
                Name = "Poison Damage Over Time Multiplier",
                ToolTipText = "Increases the damage dealt by poison effects over time.",
                Type = StatDisplayType.Multiplier,
            }
        },
        {
            StatType.PoisonDurationMultiplier,
            new StatDisplayData
            {
                Name = "Poison Duration Multiplier",
                ToolTipText = "Increases the duration of poison effects applied to enemies.",
                Type = StatDisplayType.Multiplier,
            }
        }
    };

    public static StatDisplayData GetStatDisplayData(StatType statType)
    {
        return StatTypeToDisplayDataMapping.GetValueOrDefault(statType);
    }

    public static string GetTooltipText(StatType statType)
    {
        return StatTypeToDisplayDataMapping.TryGetValue(statType, out var statDisplayData)
            ? statDisplayData.ToolTipText
            : "";
    }

    // Start is called before the first frame update
    public static float GetStatValue(StatType statType, PlayerStats playerStats)
    {
        return statType switch
        {
            // Base Stats
            StatType.Health => playerStats.MaxHealth,
            StatType.HealthRegen => playerStats.HealthRegen,
            StatType.GlobalDamageMultiplier => playerStats.BaseDamageMultiplier,
            StatType.BaseFireRateMultiplier => playerStats.BaseFireRateMultiplier,
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
            StatType.FieldsCooldownReduction => playerStats.FieldsCooldownMultiplier,

            // Specialized Stats
            StatType.PoisonChance => playerStats.PoisonChance,
            StatType.PoisonDamageOverTimeMultiplier => playerStats.PoisonDamageOverTimeMultiplier,
            StatType.PoisonDurationMultiplier => playerStats.PoisonDurationMultiplier,

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
            StatDisplayType.Time => "s",
            StatDisplayType.Duration => "/s",
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