using UnityEngine;

[CreateAssetMenu(fileName = "ChargedVoidBurstField", menuName = "Fields/Charged Void Burst Field")]
public class ChargedVoidBurstField : ChargeableField
{
    [SerializeField] private BaseAttack voidBurstAttack;
    public BaseAttack VoidBurstAttack => voidBurstAttack;

    [SerializeField] private float curseDuration;
    public float CurseDuration => curseDuration;

    [SerializeField] private float curseDamageAmount;
    public float CurseDamageAmount => curseDamageAmount;

    [SerializeField] private float curseDamageInterval;
    public float CurseDamageInterval => curseDamageInterval;
} 