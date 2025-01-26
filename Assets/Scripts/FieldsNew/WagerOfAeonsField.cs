using UnityEngine;

[CreateAssetMenu(fileName = "WagerOfAeonsField", menuName = "Fields/Wager Of Aeons Field")]
public class WagerOfAeonsField : Field
{
    [SerializeField] private WagerOfAeonsAttack wagerOfAeonsAttack;
    public WagerOfAeonsAttack WagerOfAeonsAttack => wagerOfAeonsAttack;
} 