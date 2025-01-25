using UnityEngine;
using System.Collections.Generic;

//[RequireComponent(typeof(PlayerStats))]
public class BuffManager : MonoBehaviour
{
    private Stats _stats;
    private Health _playerHealth;
    private Dictionary<string, List<BuffBase>> activeBuffs = new Dictionary<string, List<BuffBase>>();

    // For debug/inspector display:
    [SerializeField] private List<ActiveBuffDebugData> debugBuffs = new List<ActiveBuffDebugData>();
    [SerializeField] private List<ActiveBuffDebugData> debugDebuffs = new List<ActiveBuffDebugData>();

    private void Awake()
    {
        _stats = GetComponent<Stats>();
        _playerHealth = GetComponent<Health>();
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
                buff.OnUpdate(_stats, deltaTime);

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

        // If the buff is "Independent" and has a MaxStackCount >= 0, 
        // enforce that many stacks by removing oldest if we're at capacity:
        if (newBuff.StackingMode == StackingMode.Independent && newBuff.MaxStackCount >= 0)
        {
            if (buffList.Count >= newBuff.MaxStackCount)
            {
                BuffBase oldestBuff = buffList[0];
                oldestBuff.OnRemove(_stats);
                buffList.RemoveAt(0);
                Debug.Log($"Removed oldest stack of buffId={buffId} due to max stack limit={newBuff.MaxStackCount}.");
            }
        }

        if (buffList.Count == 0)
        {
            // No existing buff of this type
            AddNewBuff(newBuff);
            return;
        }

        // Otherwise handle stacking mode. For "Independent", we just add a new instance:
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
                    oldBuff.OnRemove(_stats);
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
                existingBuff.AddStack();
                // etc. Not used in this scenario
                break;
        }
    }

    private void AddNewBuff(BuffBase newBuff)
    {
        activeBuffs[newBuff.BuffId].Add(newBuff);
        newBuff.OnApply(_stats);
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
                instance.OnRemove(_stats);
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
    /// Forcibly remove all copies of a buff by ID
    /// </summary>
    public void RemoveBuffImmediately(string buffId)
    {
        if (activeBuffs.TryGetValue(buffId, out var buffList))
        {
            foreach (var b in buffList)
            {
                b.OnRemove(_stats);
            }
            activeBuffs.Remove(buffId);
        }
    }

    public int GetTotalStacksOf(string buffId)
    {
        if (!activeBuffs.TryGetValue(buffId, out var buffList))
            return 0;
        int totalStacks = 0;
        foreach (var buff in buffList)
        {
            totalStacks += buff.StackCount;
        }
        return totalStacks;
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