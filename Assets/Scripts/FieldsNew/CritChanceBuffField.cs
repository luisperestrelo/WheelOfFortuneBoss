using UnityEngine;

[CreateAssetMenu(fileName = "CritChanceBuffField", menuName = "Fields/Crit Chance Buff Field")]
public class CritChanceBuffField : Field
{
    [Header("Crit Chance Buff Settings")]
    [SerializeField] private float critChanceIncrease = 0.15f;    
    public float CritChanceIncrease => critChanceIncrease;

    [SerializeField] private float buffDuration = 6f;             
    public float BuffDuration => buffDuration;

    private void OnValidate()
    {
        if (FieldType != FieldType.CritChanceBuff)
        {
            Debug.LogWarning($"[CritChanceBuffField] FieldType was {FieldType}, changing to CritChanceBuff.");
            // fieldType = FieldType.CritChanceBuff;
        }
    }
} 