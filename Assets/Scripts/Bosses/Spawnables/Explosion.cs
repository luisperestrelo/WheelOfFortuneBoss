using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float lifeTime = 0.3f; // Duration of the explosion visual

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    public void SetLifeTime(float newLifeTime)
    {
        lifeTime = newLifeTime;
        // Update the Destroy call to use the new lifetime
        Destroy(gameObject, lifeTime);
    }
} 