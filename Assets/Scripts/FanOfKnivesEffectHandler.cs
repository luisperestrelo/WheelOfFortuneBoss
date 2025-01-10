using UnityEngine;

public class FanOfKnivesEffectHandler : FieldEffectHandler
{
    [SerializeField] private FanOfKnivesAttack fanOfKnivesAttack;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        fanOfKnivesAttack = ((FanOfKnivesField)fieldData).FanOfKnivesAttack;
    }

    public override void OnEnter(Player player)
    {
        Debug.Log("Entering Fan of Knives Field");
        player.GetComponent<PlayerCombat>().CurrentAttack = fanOfKnivesAttack;
    }

    public override void OnStay(Player player, float deltaTime)
    {
        // Nothing needed here
    }

    public override void OnExit(Player player)
    {
        Debug.Log("Exiting Fan of Knives Field");
        player.GetComponent<PlayerCombat>().CurrentAttack = null; // Reset to default attack
    }
}
