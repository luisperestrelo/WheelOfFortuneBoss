using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDamageDealerToPlayer : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 1f;
    
    private float timer = 0f;
    private bool hasHitPlayer = false;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
    
    public virtual void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out var health) && !hasHitPlayer)
        {
            health.TakeDamage(damage);
            hasHitPlayer = true;
        }
    }
}
