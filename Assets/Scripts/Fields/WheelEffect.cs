using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract ScriptableObject with methods that are called when the player interacts with a wheel segment during a fight. <br />
/// Can be inserted into any wheel segment.
/// </summary>
public abstract class WheelEffect : ScriptableObject
{
    public Sprite icon;
    public Color color;

    public virtual void OnEnterField(PlayerCombat player) { }
    public virtual void OnExitField(PlayerCombat player) { }
    public virtual void OnUpdate(PlayerCombat player) { }

    /// <summary>
    /// Adds any effect to a given projectile.
    /// </summary>
    /// <param name="projectile">The projectile being decorated.</param>
    public virtual Projectile DecorateProjectile(Projectile projectile)
    {
        return projectile;
    }
}
