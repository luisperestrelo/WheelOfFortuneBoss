using UnityEngine;

public class LaserCollision : MonoBehaviour
{
    [SerializeField] private float damageAmount = 30f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
} 