using UnityEngine;

public class ShockwaveSegment : MonoBehaviour
{
    private float speed = 5f;
    private Vector3 direction;
    private float damage = 10f;
    [SerializeField] private float lifetime = 30f;
    private Shockwave parentShockwave;
    private bool expandOutward;
    private PlayerHealth playerHealth;
    
    private void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        transform.localPosition += direction * speed * Time.deltaTime;

        //For inward shockwaves, destroy the segment when it reaches the origin;
        //But for outward, which start at 0,0, this doesn't happen. Outward just disappear once their lifetime runs out
        if (!expandOutward && Vector3.Distance(transform.localPosition, Vector3.zero) < 0.5f)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(float speed, Vector3 direction, float damage, Shockwave parentShockwave, bool expandOutward)
    {
        this.speed = speed;
        this.direction = direction;
        this.damage = damage;
        this.parentShockwave = parentShockwave;
        this.expandOutward = expandOutward;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!parentShockwave.HasDealtDamage())
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    parentShockwave.MarkAsDealtDamage();
                }
            }
        }
    }
} 


