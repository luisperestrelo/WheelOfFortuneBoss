using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Areas/Fireball Area")]
public class FireDamageArea : WheelEffect
{
    [SerializeField] private float _attackRate = 0.5f;

    [SerializeField]
    private Projectile _fireballPrefab;
    private float _nextAttackTime = 0f;

    public override void OnUpdate(PlayerCombat player)
    {
        Debug.Log("projectile is now fireball");
        base.OnUpdate(player);
        player.SetProjectileType(_fireballPrefab);
        
    }
}
