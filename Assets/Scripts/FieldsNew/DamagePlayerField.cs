using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamagePlayerField", menuName = "Fields/Damage Player Field")]
public class DamagePlayerField : Field
{
    [SerializeField] private float damageAmount;
    public float DamageAmount => damageAmount;  
    
    [SerializeField] private float damageInterval;
    public float DamageInterval => damageInterval;
    
}
