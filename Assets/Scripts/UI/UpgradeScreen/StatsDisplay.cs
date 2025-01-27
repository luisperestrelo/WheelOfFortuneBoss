using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    // Quick Solution for Tooltips
    public static Action<StatType, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;
    
    [SerializeField] private List<StatType> statsToDisplay = new List<StatType>();
    [SerializeField] private StatsDisplayItem statsTextPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private RectTransform bg;
    [SerializeField] private Tooltip tooltip;
    private List<StatsDisplayItem> statsDisplayItems = new List<StatsDisplayItem>();
    private List<StatsDisplayItem> tempStatsDisplayItems = new List<StatsDisplayItem>();


    private TextMeshProUGUI temporaryStat;
    private PlayerStats playerStats;

    private float currentY;
    private float tempStartY ;
    private float minBgSize;


    private readonly Dictionary<StatType, StatDisplayData> statsDisplayMap = new()
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
        }
    };



    // Start is called before the first frame update
    void Start()
    {
        statsToDisplay.Add(StatType.Health);
        statsToDisplay.Add(StatType.HealthRegen);
        statsToDisplay.Add(StatType.GlobalDamageMultiplier);
        statsToDisplay.Add(StatType.HealingFieldsStrength);
        playerStats = FindObjectOfType<PlayerStats>();
        minBgSize = bg.sizeDelta.y;
    }
    private void OnEnable()
    {
        OnMouseHover += ShowTooltip;
        OnMouseLoseFocus += HideTooltip;
    }


    private void OnDisable()
    {
        OnMouseHover -= ShowTooltip;
        OnMouseLoseFocus -= HideTooltip;
    }

    private void ShowTooltip(StatType type, Vector2 position)
    {
        var text = statsDisplayMap[type].ToolTipText;
        tooltip.transform.position = new Vector2(position.x + tooltip.Rect.sizeDelta.x * 2, position.y);

        tooltip.Text = text;
        tooltip.gameObject.SetActive(true);
    }

    private void HideTooltip()
    {
        tooltip.gameObject.SetActive(false);
    }
    
    public void UpdateStats()
    {
        PopulateStats();
    }
    


    private void PopulateStats()
    {
        statsDisplayItems.ForEach(x => Destroy(x.gameObject));
        statsDisplayItems.Clear();

        SetCurrentY(0f);

        statsToDisplay.ForEach(type =>
        {
            var item = AddStatDisplay(type );
            if (!item) return;
            SetCurrentY(currentY - item.Rect.rect.height);
            statsDisplayItems.Add(item);

        });
        tempStartY = currentY;
    }

    private void SetCurrentY(float value)
    {
        currentY = value;
        bg.sizeDelta = new Vector2(bg.sizeDelta.x, minBgSize - currentY);
    }
    private StatsDisplayItem AddStatDisplay(StatType type)
    {
        if (!statsDisplayMap.TryGetValue(type, out var data)) return null;

        var  item = Instantiate(statsTextPrefab, container);
        var text = $" {GetStatValue(type)}{GetDisplayTypeText(data.Type)} {data.Name} ";
        item.Initialize(type,text,bg.sizeDelta.x, new Vector2(0f, currentY) );
        return item;
    }
    private StatsDisplayItem AddStatDisplay(StatType type, float value )
    {
        if (!statsDisplayMap.TryGetValue(type, out var data)) return null;
        
        var item = Instantiate(statsTextPrefab, container);
        var text = $" + {value}{GetDisplayTypeText(data.Type)} {data.Name}";
        item.Initialize(type, text, bg.sizeDelta.x, new Vector2(0f, currentY), Color.green);
        return item;
    }


    public void AddTemporaryStats(List<StatType> types, List<float> values)
    {
        RemoveTemporaryStats();
        
        
        SetCurrentY(tempStartY);
        
        foreach (var (type, value) in types
                     .Select((type, index) => (type, values[index])))
        {
            var item = AddStatDisplay(type, value );
            if (!item) continue;
            item.Text.color = Color.green;
            SetCurrentY(currentY - item.Rect.rect.height);
            tempStatsDisplayItems.Add(item);
        }

        
    }

    public void RemoveTemporaryStats()
    {
        tempStatsDisplayItems.ForEach(x => Destroy(x.gameObject));
        tempStatsDisplayItems.Clear();
        SetCurrentY(tempStartY);

    }

    private string GetFullStatsText(StatType statType, StatDisplayData data)
    {
        return $" {GetStatValue(statType)} {data.Name} {GetDisplayTypeText(data.Type)}";
    }

    private float GetStatValue(StatType statType)
    {
        return statType switch
        {
            StatType.Health => playerStats.MaxHealth,
            StatType.GlobalDamageMultiplier => playerStats.BaseDamageMultiplier,
            StatType.BaseFireRateMultiplier => playerStats.BaseFireRateMultiplier,
            StatType.CritChance => playerStats.CritChance,
            StatType.CritMultiplier => playerStats.CritMultiplier,
            StatType.HealthRegen => playerStats.HealthRegen,
            StatType.HealingFieldsStrength => playerStats.HealingFieldsStrengthMultiplier,
            StatType.ChargeUpFieldsSpeed => playerStats.ChargeUpFieldsSpeedMultiplier,
            StatType.DecayingChargeUpFieldsDecaySlowdown => playerStats.DecayingChargeUpFieldsDecaySlowdownMultiplier,
            StatType.PositiveNegativeFieldsEffectiveness => playerStats.PositiveNegativeFieldsEffectivenessMultiplier,
            StatType.LingeringBuffFieldsDuration => playerStats.LingeringBuffFieldsDurationMultiplier,
            StatType.LingeringBuffFieldsEffectiveness => playerStats.LingeringBuffFieldsEffectivenessMultiplier,
            StatType.FieldsCooldownReduction => playerStats.FieldsCooldownMultiplier,
            StatType.AdditionalProjectilesForAttacks => playerStats.AdditionalProjectilesForAttacks,
            _ => 0f
        };
    }

    private string GetDisplayTypeText(StatDisplayType type)
    {
        return type switch
        {
            StatDisplayType.Multiplier => "X",
            StatDisplayType.Percentage => "%",
            StatDisplayType.Time => " /s",
            _ => ""
        };
    }
    
}