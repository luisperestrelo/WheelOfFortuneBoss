using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health
{
    [SerializeField] private GameObject nextBoss;
    [SerializeField] private GameObject upgradeOrb;

    private MorsoraBossController bossController;
    private bool isImmune = false;

    protected override void Awake()
    {
        base.Awake();
        bossController = GetComponent<MorsoraBossController>();
    }

    public void SetImmune(bool isImmune)
    {
        this.isImmune = isImmune;
    }

    public override bool TakeDamage(float damageAmount)
    {
        if (isImmune)
        {
            damageSource.PlayOneShot(parrySfx);
            return false;
        }
        return base.TakeDamage(damageAmount);
    }

    protected override void Die()
    {
        bossController.SpawnUpgradeOrbWithOffset(-30f);
        bossController.SpawnUpgradeOrbWithOffset(+30f);
        AbilityObjectManager.Instance.DestroyAllFatTentacles(); // TODO: Despawn animation


        OnDie.Invoke();

        //TODO: SFX/VFX etc.

        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject, 0.1f);



        //base.Die();
        //RunManager.Instance.EndFight();


    }


}
