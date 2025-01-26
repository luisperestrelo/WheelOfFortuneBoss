using UnityEngine;

public abstract class DamageOverTimeBuff : BuffBase
{
    // E.g., how much damage per second
    protected float damagePerSecond;
    
    private float tickInterval = 0.1f;  // small interval
    private float tickTimer = 0f;       // accumulates time

    public override void OnUpdate(Stats targetStats, float deltaTime)
    {
        // Accumulate time
        tickTimer += deltaTime;

        // Once we pass our interval, apply one "tick" of damage
        while (tickTimer >= tickInterval)
        {
            tickTimer -= tickInterval;

            // Calculate the damage for one tick
            float dmgThisTick = damagePerSecond * tickInterval;
            float finalDamage = dmgThisTick * targetStats.GetAggregatedDamageMultiplier();

            // Apply to Health
            var targetHealth = targetStats.GetComponent<Health>();
            if (targetHealth != null && finalDamage > 0f)
            {
                targetHealth.TakeDamage(finalDamage, true);
            }
        }

                //every frame
/*         Health targetHealth = targetStats.GetComponent<Health>();
        float damage = damagePerSecond * deltaTime;

        if (targetHealth != null && damage > 0f)
        {
            targetHealth.TakeDamage(damage);
        } */
    }
} 