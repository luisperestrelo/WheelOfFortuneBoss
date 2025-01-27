using UnityEngine;

public class StingshotAttackEffectHandler : FieldEffectHandler
{
    [SerializeField] private StingshotAttack stingshotAttack;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        stingshotAttack = ((StingshotField)fieldData).StingshotAttack;

    }

    public override void OnEnter(Player player)
    {
        Debug.Log("Entering Stingshot Field");
        player.GetComponent<PlayerCombat>().CurrentAttack = stingshotAttack;
    }

    public override void OnStay(Player player, float deltaTime)
    {
        // Nothing needed here
    }

    public override void OnExit(Player player)
    {
        Debug.Log("Exiting Stingshot Field");
        player.GetComponent<PlayerCombat>().CurrentAttack = null; // Reset to default attack
    }
} 