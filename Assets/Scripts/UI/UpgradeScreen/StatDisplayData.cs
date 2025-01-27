using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatDisplayType {
    Absolute,
    Percentage,
    Multiplier,
    Time
}

public class StatDisplayData 
{
    public string ToolTip;
    public string Name;
    public StatDisplayType Type;
    public float ValueToAdd;
    public Transform Transform;

}
