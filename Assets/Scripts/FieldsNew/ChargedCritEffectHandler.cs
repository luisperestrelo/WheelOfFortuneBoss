using UnityEngine;

public class ChargedCritEffectHandler : ChargeableFieldEffectHandler
{
    private ChargedCritField critFieldData;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        critFieldData = fieldData as ChargedCritField;
        if (critFieldData == null)
        {
            Debug.LogError("[ChargedCritEffectHandler] Provided FieldData is not ChargedCritField.");
        }
    }

    public override void OnEnter(Player player)
    {
        if (Segment != null && !Segment.IsOnCooldown)
        {
            base.OnEnter(player);
        }
    }

    public override void OnStay(Player player, float deltaTime)
    {
        if (Segment != null && !Segment.IsOnCooldown)
        {
            base.OnStay(player, deltaTime);
        }
    }

    protected override void OnChargeComplete(Player player)
    {
        // Just apply the MaxCritBuff for the specified duration
        if (player != null)
        {
            var buffManager = player.GetComponent<BuffManager>();
            if (buffManager != null)
            {
                var buff = new MaxCritBuff(
                    1.0f, // +100% crit
                    critFieldData.ChargedCritDuration
                );
                buffManager.ApplyBuff(buff);
            }
        }

        if (Segment != null)
        {
            Segment.StartCooldown();
        }
        currentChargeTime = 0f;
    }
}