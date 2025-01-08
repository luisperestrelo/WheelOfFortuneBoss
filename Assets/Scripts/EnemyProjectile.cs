using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] protected float lifeTime = 5f;
    protected float damage;
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        Destroy(gameObject, lifeTime);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
} 