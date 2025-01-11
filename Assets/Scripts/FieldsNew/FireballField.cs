using UnityEngine;

[CreateAssetMenu(fileName = "FireballField", menuName = "Fields/Fireball Field")]
public class FireballField : Field
{
    [SerializeField] private FireballAttack fireballAttack;
    public FireballAttack FireballAttack => fireballAttack;
} 