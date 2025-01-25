using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourglassTester : MonoBehaviour
{
    public Hourglass hourglass;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            hourglass.StartTimer(3f, () => { Debug.Log("TIMER COMPLETE!"); });
        }
    }
}