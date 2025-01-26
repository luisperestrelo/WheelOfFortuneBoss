using System.Collections.Generic;
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
    DamagingField,
    TentacleShieldBusterField,
    Stingshot,
    PoisonExtension,
    PoisonDetonation,
    CritChanceBuff,
    ChargedCritBuff,
    WagerOfAeons
}

public enum ActivationType
{
    OnEnter,
    OnStay,
    OnExit,
    Timed,
    Charged
}

//system similar to PoE for skill tags
public enum Tags
{
    Attack,
    Projectile,
    Buff,
    Heal,
    Shield,
    Cooldown,
    Charged,

}

[CreateAssetMenu(fileName = "New Field", menuName = "Fields/New Field")]
public class Field : ScriptableObject
{
    [SerializeField] private string fieldName;
    public string FieldName => fieldName;
    [SerializeField] private string description;
    public string Description => description;
    [SerializeField] private Sprite icon;    
    [SerializeField] private Color color;

    public Sprite Icon => icon;
    public Color Color => color;
    [SerializeField] private FieldType fieldType;
    public FieldType FieldType => fieldType;
    [SerializeField] private List<Tags> tags;
    public List<Tags> Tags => tags;
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





} 