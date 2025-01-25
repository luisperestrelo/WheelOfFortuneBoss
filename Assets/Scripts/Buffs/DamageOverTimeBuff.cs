using UnityEngine;

public abstract class DamageOverTimeBuff : BuffBase
{
    // E.g., how much damage per second
    protected float damagePerSecond;
    
    public override void OnUpdate(Health targetHealth, float deltaTime)
    {
        float damage = damagePerSecond * deltaTime;

        if (targetHealth != null && damage > 0f)
        {
            targetHealth.TakeDamage(damage);
        }
    }
} 