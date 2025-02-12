using UnityEngine;

public class HealingEffectHandler : ChargeableFieldEffectHandler
{
    private float healAmount;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        healAmount = ((HealingField)fieldData).HealAmount;
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
        Health playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount * playerStats.HealingFieldsStrengthMultiplier);
        }
        else
        {
            Debug.LogError("HealingEffectHandler::OnChargeComplete: Could not find Health component on Player.");
        }

        if (Segment != null)
        {
            Segment.StartCooldown();
        }
    }
}