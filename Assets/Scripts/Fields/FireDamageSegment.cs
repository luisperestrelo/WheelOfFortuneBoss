using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Replaces player's projectile with the fireball.
/// </summary>
[CreateAssetMenu(menuName = "Segments/Fireball Segment")]
public class FireDamageSegment : WheelEffect
{
    [SerializeField]
    private Projectile fireball;

    public override Projectile DecorateProjectile(Projectile projectile)
    {
        return fireball;
    }
}
