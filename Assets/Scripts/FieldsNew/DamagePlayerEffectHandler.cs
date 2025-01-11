using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerEffectHandler : FieldEffectHandler
{
    private float damageAmount;
    private float damageInterval;
    private float damageTimer;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        damageAmount = ((DamagePlayerField)fieldData).DamageAmount;
        damageInterval = ((DamagePlayerField)fieldData).DamageInterval;
        damageTimer = 0f;
    }

    public override void OnEnter(Player player)
    {
        damageTimer = 0f;
    }

    public override void OnStay(Player player, float deltaTime)
    {
        damageTimer += deltaTime;
        if (damageTimer >= damageInterval)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            damageTimer = 0f;
        }   

    }

    public override void OnExit(Player player)
    {
        damageTimer = 0f;
    }
}
