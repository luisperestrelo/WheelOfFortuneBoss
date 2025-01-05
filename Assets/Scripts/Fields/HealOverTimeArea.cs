using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTimeArea : WheelEffect
{

    [SerializeField] private float healPerSecond = 2f;

    public override void OnUpdate(PlayerCombat player)
    {
        base.OnUpdate(player);
        player.health.Heal(healPerSecond * Time.deltaTime);
    }
}
