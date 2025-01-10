using UnityEngine;

public class FireballAttackEffectHandler : FieldEffectHandler
{
    [SerializeField] private FireballAttack fireballAttack;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        fireballAttack = ((FireballField)fieldData).FireballAttack;
    }

    public override void OnEnter(Player player)
    {
        Debug.Log("Entering Fireball Field");
        player.GetComponent<PlayerCombat>().CurrentAttack = fireballAttack;
    }

    public override void OnStay(Player player, float deltaTime)
    {
        // Nothing needed here
    }

    public override void OnExit(Player player)
    {
        Debug.Log("Exiting Fireball Field");
        player.GetComponent<PlayerCombat>().CurrentAttack = null; // Reset to default attack
    }
} 