using UnityEngine;

[CreateAssetMenu(fileName = "New Field Card", menuName = "Cards/Field Card")]
public class FieldCard : Card
{
    public Field field;

    public override void OnEnable()
    {
        base.OnEnable();
        cardType = CardType.Field;
    }
}


