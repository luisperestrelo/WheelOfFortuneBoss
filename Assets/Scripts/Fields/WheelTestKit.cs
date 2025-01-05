using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just allows for testing the segment adding and removal features of the wheel.
/// </summary>
public class WheelTestKit : MonoBehaviour
{
    [SerializeField]
    private WheelBuilder builder;

    public void AddEffect(WheelEffect effect)
    {
        builder.StartCoroutine(builder.SetAreaRoutine(effect));
    }
}
