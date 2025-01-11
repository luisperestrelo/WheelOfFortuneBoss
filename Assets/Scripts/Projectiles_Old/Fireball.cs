using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
    public override void SetDamage(float damage)
    {
        //Realistically would be some other effect like "set on fire" or something, but this works for now.
        base.SetDamage(damage * 2);
    }
}
