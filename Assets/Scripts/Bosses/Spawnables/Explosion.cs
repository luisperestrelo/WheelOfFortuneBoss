using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float lifeTime = 0.3f; // Duration of the explosion visual
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Constructor to initialize values
    public void Initialize(float damage, float lifeTime)
    {
        this.damage = damage;
        this.lifeTime = lifeTime;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
} 