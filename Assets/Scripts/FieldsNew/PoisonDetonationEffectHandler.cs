using UnityEngine;

public class PoisonDetonationEffectHandler : ChargeableFieldEffectHandler
{
    private PoisonDetonationField poisonFieldData;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        poisonFieldData = fieldData as PoisonDetonationField;
        if (poisonFieldData == null)
        {
            Debug.LogError("[PoisonDetonationEffectHandler] Provided FieldData is not PoisonDetonationField.");
        }
    }

    protected override void OnChargeComplete(Player player)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
        {
            var enemyBuff = e.GetComponent<BuffManager>();
            if (enemyBuff != null)
            {
                int poisonStacks = enemyBuff.GetTotalStacksOf("Poison");
                if (poisonStacks > 0)
                {
                    float totalDamage = poisonStacks * poisonFieldData.DamagePerStack * playerStats.GetAggregatedDamageMultiplier();

                    // crit
                    if (Random.value < playerStats.GetAggregatedCritChance())
                    {
                        totalDamage *= playerStats.CritMultiplier;
                        PlayerCombat playerCombat = playerStats.GetComponent<PlayerCombat>();
                        if (playerCombat != null)
                        {
                            playerCombat.NotifyCrit();
                        }
                        Debug.Log("Rend Toxins CRIT!");
                    }

                    var health = e.GetComponent<Health>();
                    if (health != null)
                    {
                        health.TakeDamage(totalDamage);
                    }
                    Debug.Log($"[PoisonDetonation] Dealt {totalDamage} with {poisonStacks} poison stacks.");

                    if (poisonFieldData.RemovePoisonStacksAfterDamage)
                    {
                        enemyBuff.RemoveBuffImmediately("Poison");
                    }
                }
            }
        }

        // if (Segment != null) Segment.StartCooldown();
        currentChargeTime = 0f;
    }
} 