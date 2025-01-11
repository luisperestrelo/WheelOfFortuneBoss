using UnityEngine;

[CreateAssetMenu(fileName = "VoidBurstField", menuName = "Fields/Void Burst Field")]
public class VoidBurstField : Field
{
    [SerializeField] private float damageAmount;
    public float DamageAmount => damageAmount;

    [SerializeField] private BaseAttack voidBurstAttack;
    public BaseAttack VoidBurstAttack => voidBurstAttack;

    [SerializeField] private float curseDuration;
    public float CurseDuration => curseDuration;

    [SerializeField] private float curseDamageAmount;
    public float CurseDamageAmount => curseDamageAmount;

    [SerializeField] private float curseDamageInterval;
    public float CurseDamageInterval => curseDamageInterval;

    [SerializeField] private float maxTimeInField;
    public float MaxTimeInField => maxTimeInField;
}