using UnityEngine;

public abstract class BaseAttack : ScriptableObject
{
    public float BaseDamage = 1;
    public float FireRate = 1;  
    public abstract void PerformAttack(PlayerCombat playerCombat);
} 