using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for creating and destroying areas on the wheel.
/// </summary>
public class WheelBuilder : MonoBehaviour
{
    public static WheelBuilder instance;
    public static WheelEffect effectBeingPlaced;

    private void Start()
    {
        if (instance == null)
            DontDestroyOnLoad(this);
        else
            Destroy(gameObject);
    }

    [SerializeField]
    private int _radius = 5;
    /// <summary>
    /// Waits for any wheel area to be clicked on, then sets that area's effect to the given area effect.
    /// </summary>
    public IEnumerator SetAreaRoutine(WheelEffect areaEffect)
    {
        effectBeingPlaced = areaEffect;
        while(effectBeingPlaced != null)
        {
            yield return null;
        }
    }
}
