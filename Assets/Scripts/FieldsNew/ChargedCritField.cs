using UnityEngine;

[CreateAssetMenu(fileName = "ChargedCritField", menuName = "Fields/Charged Crit Field")]
public class ChargedCritField : ChargeableField
{
    [Header("Charged Crit Settings")]
    [SerializeField] private float chargedCritDuration = 1f;  
    public float ChargedCritDuration => chargedCritDuration;


    private void OnValidate()
    {
        if (FieldType != FieldType.ChargedCritBuff)
        {
            Debug.LogWarning($"[ChargedCritField] FieldType was {FieldType}, changing to ChargedCritBuff.");
            // fieldType = FieldType.ChargedCritBuff;
        }
    }
} 