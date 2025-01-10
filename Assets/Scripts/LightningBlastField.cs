using UnityEngine;

[CreateAssetMenu(fileName = "LightningBlastField", menuName = "Fields/Lightning Blast Field")]
public class LightningBlastField : ChargeableField
{


    [SerializeField] private GameObject lightningBlastPrefab;
    public GameObject LightningBlastPrefab => lightningBlastPrefab;
} 