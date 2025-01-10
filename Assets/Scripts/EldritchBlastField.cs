using UnityEngine;

[CreateAssetMenu(fileName = "EldritchBlastField", menuName = "Fields/Eldritch Blast Field")]
public class EldritchBlastField : ChargeableField
{
    /* [SerializeField] private float damageAmount;
    public float DamageAmount => damageAmount */ //handled in the prefab

    [SerializeField] private GameObject eldritchBlastPrefab; // Prefab for the visual effect
    public GameObject EldritchBlastPrefab => eldritchBlastPrefab;

    [SerializeField] private DamagingField damagingField; // The field to replace with after the blast
    public DamagingField DamagingField => damagingField;

    [SerializeField] private float damagingFieldDuration; // Duration of the damaging field
    public float DamagingFieldDuration => damagingFieldDuration;
} 