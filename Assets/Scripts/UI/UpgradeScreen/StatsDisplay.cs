using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

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

    private readonly List<StatsDisplayItem> statsDisplayItems = new List<StatsDisplayItem>();
    private readonly List<StatsDisplayItem> tempStatsDisplayItems = new List<StatsDisplayItem>();

    private TextMeshProUGUI temporaryStat;
    private PlayerStats playerStats;

    private float currentY;
    private float tempStartY;
    private Vector2 initialBgSize;

    public void AddStatsToListToShow(List<StatType> statTypes)
    {
        Debug.Log("AddStatsToListToShow");
        Debug.Log(statsToDisplay.Count);
        foreach (var statType in statTypes)
        {
            Debug.Log(statType + " is checked ");
        }
        
        
        foreach (var statType in statTypes.Where(statType => !statsToDisplay.Contains(statType)))
        {
            Debug.Log(statType + " added");
            statsToDisplay.Add(statType);
        }
        
        Debug.Log("statsToDisplay.Count");
        Debug.Log(statsToDisplay.Count);
        
    }


    private void Awake()
    {
        Debug.LogError("AWAKE");

        AddStatsToListToShow(new List<StatType>
        {
            StatType.Health,
            StatType.CritChance,
            StatType.CritMultiplier
        });

    }
    

    void Start()
    {
        // Enable for Testing
         //AddAllStatsToShow();

        // Always display these three stats by default:
        Debug.LogError("START");
        playerStats = FindObjectOfType<PlayerStats>();
        initialBgSize = bg.sizeDelta;
    }

    private void OnEnable()
    {
        OnMouseHover += ShowTooltip;
        OnMouseLoseFocus += HideTooltip;
        
        Debug.Log("OnEnable in stats display");
        if (playerStats)
        {
            UpdateStats();
        }
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTooltip;
        OnMouseLoseFocus -= HideTooltip;
    }

    // Can be used to test all stats at once
    private void AddAllStatsToShow()
    {
        AddStatsToListToShow(StatsHelper.StatTypeToDisplayDataMapping.Keys.ToList());
    }

    private void ShowTooltip(StatType type, Vector2 position)
    {
        var text = StatsHelper.GetTooltipText(type);
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
         playerStats = FindObjectOfType<PlayerStats>();
        initialBgSize = bg.sizeDelta;
        Debug.LogError("Update Stats");
        PopulateStats();
    }

    private void PopulateStats()
    {
        Debug.LogError("PopulateStats befoer checking player stats");

        if (!playerStats)
            return;
        Debug.LogError("PopulateStats after checking player stats");

        
        statsDisplayItems.ForEach(x => Destroy(x.gameObject));
        statsDisplayItems.Clear();

        SetCurrentY(0f);
        
        Debug.LogError("PopStats1");
        Debug.Log("PopStats1");
        Debug.Log(statsToDisplay.Count);

        if (statsToDisplay.Count == 0)
        {
            statsToDisplay.AddRange(new List<StatType>(){StatType.Health, StatType.CritChance, StatType.CritMultiplier});
        }

        statsToDisplay.ForEach(type =>
        {
            var item = AddStatDisplay(type);
            Debug.Log($"Trying stats for {item.Text}");
            if (!item)
            {
                Debug.Log($"Failed adding stats for {item.Text}");
            }
            SetCurrentY(currentY - item.Rect.rect.height);
            statsDisplayItems.Add(item);
        });
        tempStartY = currentY;
    }

    private void SetCurrentY(float value)
    {
        currentY = value;
        bg.sizeDelta = new Vector2(initialBgSize.x, initialBgSize.y - currentY);
        Debug.Log("BG Size delta" + bg.sizeDelta);
    }

    private StatsDisplayItem AddStatDisplay(StatType statType)
    {
        if (!StatsHelper.StatTypeToDisplayDataMapping.TryGetValue(statType, out var data)) return null;

        var item = Instantiate(statsTextPrefab, container);
        var text = StatsHelper.GetStatsDisplayText(statType, playerStats);
        item.Initialize(statType, text, initialBgSize.x, new Vector2(0f, currentY));
        return item;
    }

    private StatsDisplayItem AddStatDisplay(StatType type, float value)
    {
        if (!StatsHelper.StatTypeToDisplayDataMapping.TryGetValue(type, out var data)) return null;

        var item = Instantiate(statsTextPrefab, container);
        var text = StatsHelper.GetStatsDisplayText(type, value);
        item.Initialize(type, text, initialBgSize.x, new Vector2(0f, currentY), Color.green);
        return item;
    }

    public void AddTemporaryStats(List<StatType> types, List<float> values)
    {
        RemoveTemporaryStats();
        SetCurrentY(tempStartY - 16);

        foreach (var (type, value) in types
                     .Select((type, index) => (type, values[index])))
        {
            var item = AddStatDisplay(type, value);
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
}