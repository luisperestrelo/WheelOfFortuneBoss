using UnityEngine;

[CreateAssetMenu(fileName = "StingShotField", menuName = "Fields/Stingshot Field")]
public class StingshotField : Field
{
    [SerializeField] private StingshotAttack stingshotAttack;
    public StingshotAttack StingshotAttack => stingshotAttack;
} 