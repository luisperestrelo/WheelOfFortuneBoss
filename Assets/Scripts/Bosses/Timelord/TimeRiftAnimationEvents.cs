using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeRiftAnimationEvents : MonoBehaviour
{
    private TimeRift _timeRift;

    private void Awake()
    {
        _timeRift = GetComponentInParent<TimeRift>();
    }


    public void SetDamageToActive()
    {
        if (_timeRift != null)
        {
            _timeRift.EnableDamage();
        }
    }
}
