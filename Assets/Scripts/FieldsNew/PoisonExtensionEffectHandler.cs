using UnityEngine;

public class PoisonExtensionEffectHandler : ChargeableFieldEffectHandler
{
    private PoisonExtensionField poisonFieldData;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        poisonFieldData = fieldData as PoisonExtensionField;
        if (poisonFieldData == null)
        {
            Debug.LogError("[PoisonExtensionEffectHandler] Provided FieldData is not PoisonExtensionField.");
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
                enemyBuff.ExtendBuffDuration("Poison", poisonFieldData.PoisonExtensionAmount);
            }
        }

        // if (Segment != null) Segment.StartCooldown();

        // Reset charge
        currentChargeTime = 0f;
    }
} 