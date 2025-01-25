using UnityEngine;

[CreateAssetMenu(fileName = "StingShotField", menuName = "Fields/StingShot Field")]
public class StingShotField : Field
{
    [SerializeField] private StingShotAttack stingShot;
    public StingShotAttack StingShot => stingShot;
} 