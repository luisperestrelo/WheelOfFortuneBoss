using UnityEngine;

//TODO define this better
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] protected float lifeTime = 5f;
    [SerializeField] private GameObject hitVfxPrefab;

    protected float damage = 5f;
    protected float speed;
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        Destroy(gameObject, lifeTime);
    }

    protected virtual void Update()
    {
    
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
                // Get the velocity direction
                Vector2 velocity = rb.velocity.normalized;

                // Calculate the angle based on velocity direction
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 180f ;
                
                playerHealth.TakeDamage(damage);
                var hit = Instantiate(hitVfxPrefab, transform.position, Quaternion.Euler(0, 0, angle));
            }
            Destroy(gameObject);
        }
    }
} 