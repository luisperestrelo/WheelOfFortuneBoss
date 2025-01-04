using UnityEngine;

public class ShockwaveSegment : MonoBehaviour
{
    private float speed = 5f;
    private Vector3 direction;
    private float damage = 10f;
    [SerializeField] private float lifetime = 30f;
    private Transform shockwaveOrigin;
    private bool expandOutward;

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

    public void Initialize(float speed, Vector3 direction, float damage, Transform origin, bool expandOutward)
    {
        this.speed = speed;
        this.direction = direction;
        this.damage = damage;
        this.shockwaveOrigin = origin;
        this.expandOutward = expandOutward;

        Destroy(gameObject, lifetime);
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
} 


