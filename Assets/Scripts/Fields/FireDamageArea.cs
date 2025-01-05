using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamageArea : WheelArea
{
    [SerializeField] private float _attackRate = 0.5f;

    [SerializeField]
    private Projectile _fireballPrefab;
    private float _nextAttackTime = 0f;

    public override void OnUpdate(PlayerCombat player)
    {
        base.OnUpdate(player);
        player.SetProjectileType(_fireballPrefab);
    }
}
