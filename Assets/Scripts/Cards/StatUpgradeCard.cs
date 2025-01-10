using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Upgrade Card", menuName = "Cards/Stat Upgrade Card")]
public class StatUpgradeCard : Card
{
    public StatType statType;
    public float statValue;
} 