using UnityEngine;

[CreateAssetMenu(fileName = "PoisonExtensionField", menuName = "Fields/Poison Extension Field")]
public class PoisonExtensionField : ChargeableField
{

    
    [Header("Poison Extension Settings")]
    [SerializeField] private float poisonExtensionAmount = 4f;
    public float PoisonExtensionAmount => poisonExtensionAmount;

    
    private void OnValidate()
    {

        if (FieldType != FieldType.PoisonExtension)
        {
            Debug.LogWarning($"[PoisonExtensionField] FieldType was {FieldType}, changing to PoisonExtension.");
            
            // fieldType = FieldType.PoisonExtension;
        }
    }
} 
