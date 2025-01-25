using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Minimal buff manager for enemies. 
/// Does NOT interact with aggregator-based stats, or stats in general.
/// If we do want to have stuff like "Boss takes increase damage", we will just add custom code here, since bosses dont really have stats.
/// Tracks buffs (like poison) and handles their durations.
/// </summary>
public class EnemyBuffManager : MonoBehaviour
{
    // Store each buff by BuffId
    private Dictionary<string, List<BuffBase>> activeBuffs = new Dictionary<string, List<BuffBase>>();
    private Stats _stats;

    private Health _health;

    // For debug/inspector display:
    [SerializeField] private List<ActiveBuffDebugData> debugBuffs = new List<ActiveBuffDebugData>();
    [SerializeField] private List<ActiveBuffDebugData> debugDebuffs = new List<ActiveBuffDebugData>();

    private void Awake()
    {
        _health = GetComponent<Health>();
        _stats = GetComponent<Stats>();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        List<(string, BuffBase)> buffsToRemove = null;

        // Update each buff's duration
        foreach (var kvp in activeBuffs)
        {
            var buffList = kvp.Value;

            foreach (var buff in buffList)
            {
                buff.OnUpdate(_stats, deltaTime);

                // Decrement buff's duration
                if (buff.UpdateDuration(deltaTime))
                {
                    // Mark for removal if expired
                    if (buffsToRemove == null)
                        buffsToRemove = new List<(string, BuffBase)>();
                    buffsToRemove.Add((kvp.Key, buff));
                }
            }
        }

        // Remove expired buffs
        if (buffsToRemove != null)
        {
            foreach ((string buffId, BuffBase buff) in buffsToRemove)
            {
                RemoveBuffInstance(buffId, buff);
            }
        }

        // Refresh our debug list so we can see it in the inspector
        RefreshDebugList();
    }

    /// <summary>
    /// Apply a new buff. This manager does not handle aggregator logic; 
    /// we simply track durations, stacking, etc.
    /// </summary>
    public void ApplyBuff(BuffBase newBuff)
    {
        string buffId = newBuff.BuffId;

        // Check if we already have a list for this buffId
        if (!activeBuffs.TryGetValue(buffId, out var buffList))
        {
            buffList = new List<BuffBase>();
            activeBuffs[buffId] = buffList;
        }

        // Standard approach: if the buff mode is "Independent," 
        // we simply add a new instance. If user wants special logic 
        // (like max stacks), they'd copy from the player's BuffManager code.
        if (buffList.Count == 0)
        {
            AddNewBuff(newBuff);
            return;
        }

        switch (newBuff.StackingMode)
        {
            case StackingMode.Independent:
                AddNewBuff(newBuff);
                break;

            case StackingMode.ReplaceOldWithNew:
                // Remove each old instance
                foreach (var oldBuff in buffList)
                {
                    oldBuff.OnRemove(null);
                }
                buffList.Clear();
                // Add the new instance
                AddNewBuff(newBuff);
                break;

            case StackingMode.RefreshDuration:
                // Just refresh the first instance's duration. 
                var firstBuff = buffList[0];
                firstBuff.Duration = newBuff.Duration;
                break;

            case StackingMode.IncrementStack:
                var existingBuff = buffList[0];
                existingBuff.AddStack();
                break;
        }
    }

    private void AddNewBuff(BuffBase buff)
    {
        activeBuffs[buff.BuffId].Add(buff);
        buff.OnApply(null);
        // We pass null instead of stats since we don't handle aggregator logic for enemies
    }

    /// <summary>
    /// Remove a single buff instance from the dictionary
    /// </summary>
    private void RemoveBuffInstance(string buffId, BuffBase instance)
    {
        if (activeBuffs.TryGetValue(buffId, out var buffList))
        {
            if (buffList.Contains(instance))
            {
                instance.OnRemove(null);
                buffList.Remove(instance);
            }

            if (buffList.Count == 0)
            {
                activeBuffs.Remove(buffId);
            }
        }
    }

    /// <summary>
    /// Forcibly remove all copies of a buff by ID (e.g. all poison stacks)
    /// </summary>
    public void RemoveBuffImmediately(string buffId)
    {
        if (activeBuffs.TryGetValue(buffId, out var buffList))
        {
            foreach (var b in buffList)
            {
                b.OnRemove(null);
            }
            activeBuffs.Remove(buffId);
        }
    }

    /// <summary>
    /// Returns how many total "stacks" of a given buff ID exist.
    /// If using StackingMode.Independent with N instances, each 
    /// typically has StackCount=1 => total = buffList.Count.
    /// If using IncrementStack, it might be a single buff with .StackCount
    /// </summary>
    public int GetTotalStacksOf(string buffId)
    {
        if (!activeBuffs.TryGetValue(buffId, out var buffList))
            return 0;

        int total = 0;
        foreach (var buff in buffList)
        {
            total += buff.StackCount;
        }
        return total;
    }

    

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

