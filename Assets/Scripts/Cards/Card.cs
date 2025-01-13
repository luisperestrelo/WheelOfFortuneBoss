using UnityEngine;

public enum CardRarity { Common, Rare, Epic }
public enum StatType
{
    Health,
    GlobalDamageMultiplier,
    BaseFireRateMultiplier,
    CritChance,
    CritMultiplier,
    HealthRegen,

    // Field-related stats  
    HealingFieldsStrength,
    ChargeUpFieldsSpeed,
    DecayingChargeUpFieldsDecaySlowdown,
    PositiveNegativeFieldsEffectiveness,
    LingeringBuffFieldsDuration,
    FieldsCooldownReduction,
    //ProjectileReplacingFieldsAdditionalProjectiles, //do this later, when we rework projectiles
}
public enum CardType { Field, StatUpgrade, FieldCategoryUpgrade, FieldSpecificUpgrade, WheelUpgrade }
// maybe wheleupgrade would be stuff like "choose a field, and increase its size"



//noit using this stuff yet
public enum UpgradeCategory { ChargeSpeed, Damage, Cooldown }
public enum FieldUpgradeType { Damage, Cooldown, Duration, ChargeTime, CurseDuration, CurseDamage, MaxTimeInField }

public abstract class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public CardRarity rarity;
    public Sprite icon;
    public CardType cardType;
}