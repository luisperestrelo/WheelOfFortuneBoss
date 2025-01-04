using UnityEngine;

public class ShockwaveSegment : MonoBehaviour
{
    private float speed = 5f;
    private Vector3 direction;
    private float damage = 10f;
    private float lifetime = 3f;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Initialize(float speed, Vector3 direction, float damage)
    {
        this.speed = speed;
        this.direction = direction;
        this.damage = damage;

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


