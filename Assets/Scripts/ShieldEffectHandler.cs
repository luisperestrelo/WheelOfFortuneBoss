using UnityEngine;

public class ShieldEffectHandler : FieldEffectHandler
{
    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
    }

    public override void OnEnter(Player player)
    {
        if (Segment != null && !Segment.IsOnCooldown)
        {
            Debug.Log("Entering Shield Field");
            player.GetComponent<PlayerCombat>().ActivateShield(null, null); // Activate the shield
            Segment.StartCooldown(); // Start the cooldown
        }
    }

    public override void OnStay(Player player, float deltaTime)
    {
        // Nothing needed here for a simple shield
    }

    public override void OnExit(Player player)
    {
        Debug.Log("Exiting Shield Field");
        player.GetComponent<PlayerCombat>().RemoveShield(); // Remove the shield when exiting
    }
} 