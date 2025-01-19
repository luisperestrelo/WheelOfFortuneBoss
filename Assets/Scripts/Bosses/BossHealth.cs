using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health
{
    private bool isImmune = false;

    public void SetImmune(bool isImmune)
    {
        this.isImmune = isImmune;
    }

    public override void TakeDamage(float damageAmount)
    {
        if (isImmune)
        {
            return;
        }
        base.TakeDamage(damageAmount);
    }

    protected override void Die()
    {
        base.Die();
        RunManager.Instance.EndFight();
    }
}
