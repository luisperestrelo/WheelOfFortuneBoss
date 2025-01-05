using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private PlayerFlashFX playerFlashFX;
    [SerializeField] private PlayerSFX playerSFX;

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);

        playerFlashFX.PlayFlashFX();
        playerSFX.PlayTakeDamageClip();
        
    }   
}
