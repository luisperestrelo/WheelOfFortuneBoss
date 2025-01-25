using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerStats))]
public class BuffManager : MonoBehaviour
{
    private PlayerStats _playerStats;
    private Dictionary<string, List<BuffBase>> activeBuffs = new Dictionary<string, List<BuffBase>>();

    // For debug/inspector display:
    [SerializeField] private List<ActiveBuffDebugData> debugBuffs = new List<ActiveBuffDebugData>();
    [SerializeField] private List<ActiveBuffDebugData> debugDebuffs = new List<ActiveBuffDebugData>();

    private void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        // We'll gather a list of buffs to remove
        List<(string, BuffBase)> buffsToRemove = null;

        // Now we have a list per BuffId
        foreach (var kvp in activeBuffs)
        {
            var buffList = kvp.Value;
            foreach (var buff in buffList)
            {
                buff.OnUpdate(_playerStats, deltaTime);

                if (buff.UpdateDuration(deltaTime))
                {
                    if (buffsToRemove == null) buffsToRemove = new List<(string, BuffBase)>();
                    buffsToRemove.Add((kvp.Key, buff));
                }
            }
        }

        if (buffsToRemove != null)
        {
            foreach (var tuple in buffsToRemove)
            {
                RemoveBuffInstance(tuple.Item1, tuple.Item2);
            }
        }

        // Refresh our debug list so we can see it in the inspector
        RefreshDebugList();
    }

    /// <summary>
    /// Apply a new buff using that Buff's StackingMode.
    /// </summary>
    public void ApplyBuff(BuffBase newBuff)
    {
        string buffId = newBuff.BuffId;

        // See if we have any existing buffs with this BuffId
        if (!activeBuffs.TryGetValue(buffId, out var buffList))
        {
            // No buff with that ID, so create a new list
            buffList = new List<BuffBase>();
            activeBuffs[buffId] = buffList;
        }

        if (buffList.Count == 0)
        {
            // No existing buff of this type
            AddNewBuff(newBuff);
            return;
        }

        // If we do have an existing buff(s), handle stacking logic:
        switch (newBuff.StackingMode)
        {
            case StackingMode.Independent:
                // Simply add a new instance
                AddNewBuff(newBuff);
                break;

            case StackingMode.ReplaceOldWithNew:
                // Remove each old instance
                foreach (var oldBuff in buffList)
                {
                    oldBuff.OnRemove(_playerStats);
                }
                buffList.Clear();
                // Now add the new instance
                AddNewBuff(newBuff);
                break;

            case StackingMode.RefreshDuration:
                // We just pick the first old buff (or all) to refresh
                // for this example, assume 1 buff
                var firstBuff = buffList[0];
                firstBuff.Duration = newBuff.Duration;
                // We do NOT re-apply stats => we keep oldBuff's stats
                break;

            case StackingMode.IncrementStack:
                // Potentially each BuffBase has a "StackCount" property
                // we'd do oldBuff.IncrementStack() or something
                // For demonstration, assume we'll keep track in the buff itself
                var existingBuff = buffList[0];
                // existingBuff.IncrementStack(...);
                // Also possibly reset or max out the duration, etc.
                break;
        }
    }

    private void AddNewBuff(BuffBase newBuff)
    {
        activeBuffs[newBuff.BuffId].Add(newBuff);
        newBuff.OnApply(_playerStats);
    }

    /// <summary>
    /// Called when a single BuffBase instance ends
    /// </summary>
    private void RemoveBuffInstance(string buffId, BuffBase instance)
    {
        if (activeBuffs.TryGetValue(buffId, out var buffList))
        {
            if (buffList.Contains(instance))
            {
                instance.OnRemove(_playerStats);
                buffList.Remove(instance);
            }

            // If no more buff instances of this ID, remove the dictionary entry
            if (buffList.Count == 0)
            {
                activeBuffs.Remove(buffId);
            }
        }
    }

    /// <summary>
    /// If you want to forcibly remove all copies of a buff by ID
    /// </summary>
    public void RemoveBuffImmediately(string buffId)
    {
        if (activeBuffs.TryGetValue(buffId, out var buffList))
        {
            foreach (var b in buffList)
            {
                b.OnRemove(_playerStats);
            }
            activeBuffs.Remove(buffId);
        }
    }

    /// <summary>
    /// Can expand in the inspector to show info about current buffs
    /// </summary>
    private void RefreshDebugList()
    {
        debugBuffs.Clear();
        debugDebuffs.Clear();

        foreach (var kvp in activeBuffs)
        {
            foreach (var buff in kvp.Value)
            {
                var data = new ActiveBuffDebugData
                {
                    BuffId = buff.BuffId,
                    CurrentDuration = buff.Duration,
                    BuffType = buff.BuffType
                };

                if (buff.BuffType == BuffType.Buff)
                    debugBuffs.Add(data);
                else
                    debugDebuffs.Add(data);
            }
        }
    }
}

[System.Serializable]
public struct ActiveBuffDebugData
{
    public string BuffId;
    public float CurrentDuration;
    public BuffType BuffType;
} 