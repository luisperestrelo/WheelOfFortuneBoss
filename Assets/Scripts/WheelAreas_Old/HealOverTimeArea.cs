using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTimeArea : WheelArea
{

    [SerializeField] private float healPerSecond = 2f;


    protected override void Update()
    {
        base.Update();
        Debug.Log("Player is in area name: " + gameObject.name);

        if (_isPlayerInArea && _playerHealth != null)
        {
            _playerHealth.Heal(healPerSecond * Time.deltaTime);
        }
    }
}
