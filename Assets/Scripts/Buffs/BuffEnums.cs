public enum BuffType
{
    Buff,
    Debuff
}

public enum StackingMode
{
    /// <summary>
    /// Always create a brand new instance (independent stacks).
    /// e.g. For Poison, each application is a separate DoT instance.
    /// </summary>
    Independent,

    /// <summary>
    /// Remove the old buff, then apply the new buff.
    /// This ensures the newly applied buff's stats/duration override previous.
    /// </summary>
    ReplaceOldWithNew,

    /// <summary>
    /// Keep the old buff, but refresh its duration only.
    /// (Meaning we re-use the old buff's stats and re-set the timer to the new duration.)
    /// </summary>
    RefreshDuration,

    /// <summary>
    /// Increase an internal "stack count" and recalculates stats accordingly.
    /// (Requires internal logic to handle how stats scale with stack count.)
    /// </summary>
    IncrementStack,

    /// <summary>
    /// Same as Independent, but refresh all existing durations.
    /// </summary>
    IndependentButRefreshesAll
}
