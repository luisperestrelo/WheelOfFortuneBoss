using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health
{
    [SerializeField] private GameObject nextBoss;
    [SerializeField] private GameObject upgradeOrb;
    [SerializeField] private AudioClip dieSfx;

    private MorsoraBossController bossController;
    private TimelordBossController timelordBossController;
    private bool isImmune = false;

    protected override void Awake()
    {
        base.Awake();
        bossController = GetComponent<MorsoraBossController>();
        timelordBossController = GetComponent<TimelordBossController>();
    }

    public void SetImmune(bool isImmune)
    {
        this.isImmune = isImmune;
    }

    public override bool TakeDamage(float damageAmount, bool isDamageOverTime = false, bool isCrit = false)
    {
        if (isImmune)
        {
            if (!isDamageOverTime)
            {
                damageSource.clip = parrySfx;
                damageSource.Play();
            }
            return false;
        }
        return base.TakeDamage(damageAmount, isDamageOverTime, isCrit);
    }

    protected override void Die()
    {
        if (bossController != null)
        {
            bossController.SpawnUpgradeOrbWithOffset(-30f);
            bossController.SpawnUpgradeOrbWithOffset(+30f);
        }
        else if (timelordBossController != null)
        {
            timelordBossController.SpawnUpgradeOrbWithOffset(-30f);
            timelordBossController.SpawnUpgradeOrbWithOffset(+30f);
        }

        //TODO: this is a shitty temporary solution
        else
        {
            TargetDummy targetDummy = GetComponent<TargetDummy>();
            if (targetDummy != null)
            {
                targetDummy.SpawnUpgradeOrbWithOffset(0);
            }
        }

        if (AbilityObjectManager.Instance != null)
        {
            AbilityObjectManager.Instance.DestroyAllFatTentacles(); // TODO: Despawn animation
            AbilityObjectManager.Instance.DestroyAllFlails(); // TODO: Despawn animation
            AbilityObjectManager.Instance.DisableAllPortals();
            AbilityObjectManager.Instance.DestroyAllBigWavesOfDoom();
            AbilityObjectManager.Instance.DisableAllRotatingOrbs();
            AbilityObjectManager.Instance.DestroyAllTelegraphs();
        }


        OnDie.Invoke();

        //TODO: SFX/VFX etc.
        SFXPool.instance.PlaySound(dieSfx, SFXPool.MixGroup.ui);

        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject, 0.1f);

        if (AbilityObjectManager.Instance != null)
        {
            AbilityObjectManager.Instance.DestroyAllFatTentacles(); // TODO: Despawn animation
            AbilityObjectManager.Instance.DestroyAllFlails(); // TODO: Despawn animation
            AbilityObjectManager.Instance.DisableAllPortals();
            AbilityObjectManager.Instance.DestroyAllBigWavesOfDoom();
            AbilityObjectManager.Instance.DisableAllRotatingOrbs();
            AbilityObjectManager.Instance.DestroyAllTelegraphs();
        }




        //base.Die();
        //RunManager.Instance.EndFight();


    }


}
