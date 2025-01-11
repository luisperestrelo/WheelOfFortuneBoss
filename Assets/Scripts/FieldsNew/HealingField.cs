using UnityEngine;

[CreateAssetMenu(fileName = "HealingField", menuName = "Fields/Healing Field")]
public class HealingField : ChargeableField
{


    [SerializeField] private float healAmount;
    public float HealAmount => healAmount;
} 