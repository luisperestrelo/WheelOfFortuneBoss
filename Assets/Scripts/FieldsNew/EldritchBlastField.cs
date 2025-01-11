using UnityEngine;

[CreateAssetMenu(fileName = "EldritchBlastField", menuName = "Fields/Eldritch Blast Field")]
public class EldritchBlastField : ChargeableField
{
    /* [SerializeField] private float damageAmount;
    public float DamageAmount => damageAmount */ //handled in the prefab

    [SerializeField] private GameObject eldritchBlastPrefab; 
    public GameObject EldritchBlastPrefab => eldritchBlastPrefab;

    [SerializeField] private DamagingField damagingField; // The field to replace with after the blast
    public DamagingField DamagingField => damagingField;

    [SerializeField] private float damagingFieldDuration; 
    public float DamagingFieldDuration => damagingFieldDuration; // TODO: Consider making this = Cooldown, so that Cooldown upgrades affect this
} 