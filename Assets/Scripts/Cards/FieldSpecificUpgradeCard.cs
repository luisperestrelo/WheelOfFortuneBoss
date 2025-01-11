using UnityEngine;

[CreateAssetMenu(fileName = "New Field Specific Upgrade Card", menuName = "Cards/Field Specific Upgrade Card")]
public class FieldSpecificUpgradeCard : Card
{
    public FieldType targetFieldType;
    public FieldUpgradeType fieldUpgradeType;
    public float fieldUpgradeValue;
} 