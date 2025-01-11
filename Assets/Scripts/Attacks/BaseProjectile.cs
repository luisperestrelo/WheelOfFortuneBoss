using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float damage = 10f;

    protected Vector2 velocity;

    public virtual void SetVelocity(Vector2 newVelocity)
    {
        velocity = newVelocity;
    }

    public virtual void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    protected virtual void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision with enemies or other objects
        if (collision.gameObject.TryGetComponent<Health>(out var health) && health.gameObject.tag != "Player")
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}