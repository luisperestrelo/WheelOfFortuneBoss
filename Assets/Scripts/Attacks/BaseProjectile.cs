using UnityEngine;
using UnityEngine.Rendering;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] private ParticleSystem hitVfxPrefab;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private bool isCrit = false;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private bool shouldDestroyOnHit = true;

    [Space] // TODO: Refactor properly to handle multiple on hit effects
    [SerializeField] private float poisonChance = 0f;
    [SerializeField] private float poisonDamage = 0f;
    [SerializeField] private float poisonDuration = 0f;

    private float lifeTimeCounter = 0f;

    protected Vector2 velocity;

    public virtual void SetVelocity(Vector2 newVelocity)
    {
        velocity = newVelocity;
    }

    public virtual void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    public virtual void SetPoisonStats(float poisonChance, float damage, float duration)
    {
        this.poisonChance = poisonChance;
        this.poisonDamage = damage;
        this.poisonDuration = duration;
    }


    protected virtual void Update()
    {
        transform.Translate(velocity * Time.deltaTime);

        lifeTimeCounter += Time.deltaTime;
        if (lifeTimeCounter > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out var health) && health.gameObject.tag != "Player")
        {
            if (hitVfxPrefab)
            {
                var hit = Instantiate(hitVfxPrefab, transform.position, Quaternion.identity);
                hit.GetComponent<LayerSort>()?.SortToBossLayer();
            }
            health.TakeDamage(damage);

            if (poisonChance > 0f && Random.value <= poisonChance)
            {
                BuffManager manager = health.GetComponent<BuffManager>();
                if (manager != null)
                {
                    manager.ApplyBuff(new PoisonBuff(poisonDamage, poisonDuration));
                }
            }

            if (shouldDestroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }
}