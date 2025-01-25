using UnityEngine;

public abstract class DamageOverTimeBuff : BuffBase
{
    // E.g., how much damage per second
    protected float damagePerSecond;
    
    public override void OnUpdate(PlayerStats targetStats, float deltaTime)
    {
        float damage = damagePerSecond * deltaTime;

        var health = targetStats.GetComponent<Health>();
        if (health != null && damage > 0f)
        {
            health.TakeDamage(damage);
        }
    }
} 