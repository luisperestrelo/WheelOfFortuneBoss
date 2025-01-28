using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatDisplayType
{
    Absolute,
    Percentage,
    Multiplier,
    Time,
    Duration,
    NoValue
}

public class StatDisplayData
{
    public string ToolTipText;
    public string Name;
    public StatDisplayType Type;
    public bool FlippedSign; // if positive values need to displayed with "-" or vice versa
}