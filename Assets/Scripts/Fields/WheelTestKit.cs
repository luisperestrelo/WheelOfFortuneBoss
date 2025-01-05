using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just allows for testing the segment adding and removal features of the wheel.
/// </summary>
public class WheelTestKit : MonoBehaviour
{
    [SerializeField]
    private WheelArea testSeg;

    [SerializeField]
    private WheelBuilder builder;

    public void AddArea(WheelArea prefab)
    {
        builder.StartCoroutine(builder.PlaceAreaRoutine(prefab));
    }
}
