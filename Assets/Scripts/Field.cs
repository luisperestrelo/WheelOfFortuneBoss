using UnityEngine;

public enum FieldType
{
    DamageBuff,
    Fireball,
    LightningBlast,
    Shield,
    Heal,
    VampiricSeeds,
    BossDebuff,
    VoidBurst,
    ChargedVoidBurst,
    EldritchBlast,
    FanOfKnives,
    DamagePlayer,
    DamagingField
    // Add other field types here
}

public enum ActivationType
{
    OnEnter,
    OnStay,
    OnExit,
    Timed,
    Charged
}

[CreateAssetMenu(fileName = "New Field", menuName = "Fields/New Field")]
public class Field : ScriptableObject
{
    [SerializeField] private string fieldName;
    public string FieldName => fieldName;
    [SerializeField] private string description;
    public string Description => description;
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
    [SerializeField] private FieldType fieldType;
    public FieldType FieldType => fieldType;
    [SerializeField] private ActivationType activationType;
    public ActivationType ActivationType => activationType;
    [SerializeField] private float duration;
    public float Duration => duration;
    [SerializeField] private float cooldown;
    public float Cooldown => cooldown;
    [SerializeField] private GameObject visualEffect;
    public GameObject VisualEffect => visualEffect;
    [SerializeField] private AudioClip audioEffect;
    public AudioClip AudioEffect => audioEffect;
    [SerializeField] private float size = 1f;
    public float Size => size;

    // Add other data properties specific to the field type here
    // For example:
    // [SerializeField] private float damageMultiplier;
    // public float DamageMultiplier => damageMultiplier;
    // [SerializeField] private float healAmount;
    // public float HealAmount => healAmount;
    // [SerializeField] private float debuffStrength;
    // public float DebuffStrength => debuffStrength;
} 