using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Areas/Heal Area")]
public class HealOverTimeArea : WheelEffect
{

    [SerializeField] private float healPerSecond = 2f;

    public override void OnUpdate(PlayerCombat player)
    {
        base.OnUpdate(player);
        player.health.Heal(healPerSecond * Time.deltaTime);
    }
}
