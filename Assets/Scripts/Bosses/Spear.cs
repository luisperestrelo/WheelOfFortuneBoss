using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime); 
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
            Destroy(gameObject);
        }
    }

    // Add setters for speed, damage, and lifetime
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
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
