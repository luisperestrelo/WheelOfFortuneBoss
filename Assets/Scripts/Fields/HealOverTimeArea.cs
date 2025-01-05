using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTimeArea : WheelArea
{

    [SerializeField] private float healPerSecond = 2f;

    public override void OnUpdate(PlayerCombat player)
    {
        base.OnUpdate(player);
        Debug.Log("Player is in area name: " + gameObject.name);

        player.health.Heal(healPerSecond * Time.deltaTime);
    }
}
