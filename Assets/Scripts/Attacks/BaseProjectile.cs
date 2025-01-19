using UnityEngine;
using UnityEngine.Rendering;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] private ParticleSystem hitVfxPrefab;

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
        if (collision.gameObject.TryGetComponent<Health>(out var health) && health.gameObject.tag != "Player")
        {
            Debug.Log("hit");
            var hit = Instantiate(hitVfxPrefab, transform.position, Quaternion.identity);
            hit.GetComponent<AutoLayerSort>()?.SetSortingLayer();
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}