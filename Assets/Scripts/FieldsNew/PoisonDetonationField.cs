using UnityEngine;

[CreateAssetMenu(fileName = "PoisonDetonationField", menuName = "Fields/Poison Detonation Field")]
public class PoisonDetonationField : ChargeableField
{
    [Header("Poison Detonation Settings")]
    [SerializeField] private float damagePerStack = 5f;
    public float DamagePerStack => damagePerStack;

    [SerializeField] private bool removePoisonStacksAfterDamage = false;
    public bool RemovePoisonStacksAfterDamage => removePoisonStacksAfterDamage;

    private void OnValidate()
    {
        if (FieldType != FieldType.PoisonDetonation)
        {
            Debug.LogWarning($"[PoisonDetonationField] FieldType was {FieldType}, changing to PoisonDetonation.");
            // fieldType = FieldType.PoisonDetonation;
        }
    }
} 