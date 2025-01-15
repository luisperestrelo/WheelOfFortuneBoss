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
    LingeringBuffFieldsEffectiveness,
    FieldsCooldownReduction,
    AdditionalProjectilesForAttacks,

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
    public bool isKissCurse = false; //cards that are "kiss curse" cards are cards that have an upside and a downside
                                     // they should be especially highlighted in the card selection UI

    public virtual void OnValidate()
    {
    }

    public virtual void OnEnable()
    {
    }
}