using UnityEngine;

public abstract class BuffBase
{
    public string BuffId { get; protected set; }

    public BuffType BuffType { get; protected set; }

    public float Duration { get;  set; }

    public StackingMode StackingMode { get; protected set; }

    public abstract void OnApply(PlayerStats targetStats);

    public virtual void OnUpdate(PlayerStats targetStats, float deltaTime) { }

    public abstract void OnRemove(PlayerStats targetStats);

    public virtual bool UpdateDuration(float deltaTime)
    {
        Duration -= deltaTime;
        return Duration <= 0f;
    }
} 