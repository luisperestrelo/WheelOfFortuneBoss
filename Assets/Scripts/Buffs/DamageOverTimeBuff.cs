using UnityEngine;

public abstract class DamageOverTimeBuff : BuffBase
{
    // E.g., how much damage per second
    protected float damagePerSecond;
    
    public override void OnUpdate(Stats targetStats, float deltaTime)
    {
        Health targetHealth = targetStats.GetComponent<Health>();
        float damage = damagePerSecond * deltaTime;

        if (targetHealth != null && damage > 0f)
        {
            targetHealth.TakeDamage(damage);
        }
    }
} 