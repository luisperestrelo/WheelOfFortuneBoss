using UnityEngine;

public class FireballProjectile : BaseProjectile
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); 

        // Add any additional behavior specific to the fireball here like if we want it to apply burn,  or explode or w/e
        // Example:
        // if (collision.gameObject.CompareTag("Enemy"))
        // {
        //     // Apply a burn effect or an explosion
        // }
    }
} 