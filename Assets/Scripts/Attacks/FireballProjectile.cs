using UnityEngine;

public class FireballProjectile : BaseProjectile
{
    // Add any specific behavior for the fireball projectile here
    // For example, you might want to add a visual effect, a sound effect,
    // or make it deal damage over time or in an area of effect.

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Call the base method to handle default collision behavior

        // Add any additional behavior specific to the fireball here
        // Example:
        // if (collision.gameObject.CompareTag("Enemy"))
        // {
        //     // Apply a burn effect or an explosion
        // }
    }
} 