using UnityEngine;

[CreateAssetMenu(fileName = "DamagingField", menuName = "Fields/Damaging Field")]
public class DamagingField : Field
{
    [SerializeField] private float damageAmount;
    public float DamageAmount => damageAmount;

    [SerializeField] private float damageInterval;
    public float DamageInterval => damageInterval;
} 