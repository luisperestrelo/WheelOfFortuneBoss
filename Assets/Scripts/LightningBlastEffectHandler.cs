using UnityEngine;

public class LightningBlastEffectHandler : ChargeableFieldEffectHandler
{
    private GameObject lightningBlastPrefab;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        lightningBlastPrefab = ((LightningBlastField)fieldData).LightningBlastPrefab;
    }

    protected override void OnChargeComplete(Player player)
    {
        // Find the boss and instantiate the lightning blast
        BossController boss = FindObjectOfType<BossController>();
        Vector3 offset = boss.transform.Find("Sprite").localPosition;

        if (boss != null)
        {
            Instantiate(lightningBlastPrefab, boss.transform.position + offset, Quaternion.identity);
        }
        else
        {
            Debug.LogError("LightningBlastEffectHandler::OnChargeComplete: Could not find Boss in the scene.");
        }
    }
}