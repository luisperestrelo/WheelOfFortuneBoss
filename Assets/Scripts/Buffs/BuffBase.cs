using UnityEngine;

public abstract class BuffBase
{
    public string BuffId { get; protected set; }

    public BuffType BuffType { get; protected set; }

    public float Duration { get;  set; }

    public StackingMode StackingMode { get; protected set; }

    public int StackCount { get; protected set; } = 1;

    public int MaxStackCount { get; protected set; } = -1;

    public abstract void OnApply(PlayerStats targetStats);

    public virtual void OnUpdate(Health targetHealth, float deltaTime) { }

    public abstract void OnRemove(PlayerStats targetStats);

    public virtual bool UpdateDuration(float deltaTime)
    {
        Duration -= deltaTime;
        return Duration <= 0f;
    }

    public virtual void AddStack()
    {
        if (MaxStackCount < 0)
        {
            StackCount++;
        }
        else
        {
            StackCount = Mathf.Min(StackCount + 1, MaxStackCount);
        }
    }
} 