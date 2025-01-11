using UnityEngine;

[CreateAssetMenu(fileName = "DamageBuffField", menuName = "Fields/Damage Buff Field")]
public class DamageBuffField : Field
{
    [SerializeField] private float damageMultiplier;
    public float DamageMultiplier => damageMultiplier;
} 