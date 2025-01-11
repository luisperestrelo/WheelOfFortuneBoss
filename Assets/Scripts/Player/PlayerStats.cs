using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float BaseDamage { get; private set; } = 1f;
    // public float MaxHealth { get; private set; } = 100f;
     public float MaxHealth = 100f;

    public Dictionary<Tags, float> CategoryUpgrades = new Dictionary<Tags, float>();

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        // Initialize default values 
        // Save/Load stuff probably goes here
        //TODO: just a lot of todo stuff for player stats
    }

    public void IncreaseBaseDamage(float amount)
    {
        BaseDamage += amount;
        GetComponent<PlayerCombat>().UpdateBaseDamageMultiplier(BaseDamage);
        
    }

    public void MultiplyBaseDamage(float multiplier)
    {
        BaseDamage *= multiplier;
        GetComponent<PlayerCombat>().UpdateBaseDamageMultiplier(BaseDamage);
    }

    public void IncreaseMaxHealth(float amount)
    {
        MaxHealth += amount;
        GetComponent<PlayerHealth>().SetMaxHealth(MaxHealth);
    }

    public void DecreaseMaxHealth(float amount)
    {
        MaxHealth -= amount;
        if (MaxHealth <= 0)
        {
            MaxHealth = 1;
        }
        GetComponent<PlayerHealth>().SetMaxHealth(MaxHealth);
    }


} 