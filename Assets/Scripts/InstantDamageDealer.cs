using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDamageDealer : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float poisonChance = 0f;
    [SerializeField] private float poisonDamage = 0f;
    [SerializeField] private float poisonDuration = 0f;

    private float timer = 0f;

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

    public virtual void SetPoisonStats(float poisonChance, float poisonDamage, float poisonDuration)
    {
        this.poisonChance = poisonChance;
        this.poisonDamage = poisonDamage;
        this.poisonDuration = poisonDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out var health) && health.gameObject.tag != "Player")
        {
            health.TakeDamage(damage);
        }
    }
}
