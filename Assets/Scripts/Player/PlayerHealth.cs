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

        //1/10 of the player's hp does nothing, 1/3 of the player's max hp is considered max intensity.
        if (damageAmount > (GetMaxHealth() / 10))
            MusicPlayer.instance.FilterMusic(Mathf.Max(1, damageAmount / (GetMaxHealth() * 3f)));
        
    }   
}
