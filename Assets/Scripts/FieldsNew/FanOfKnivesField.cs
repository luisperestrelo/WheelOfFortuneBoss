using UnityEngine;

[CreateAssetMenu(fileName = "FanOfKnivesField", menuName = "Fields/Fan of Knives Field")]
public class FanOfKnivesField : Field
{
    [SerializeField] private FanOfKnivesAttack fanOfKnivesAttack;
    public FanOfKnivesAttack FanOfKnivesAttack => fanOfKnivesAttack;
}
