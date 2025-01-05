using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBlast : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private PlayerCombat playerCombat;

    private void Start()
    {
        Destroy(gameObject, lifetime);
        playerCombat = FindObjectOfType<PlayerCombat>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(DelayedDamage(other, 0.1f));
    }

    private IEnumerator DelayedDamage(Collider2D other, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (other != null && other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage * playerCombat.GetGlobalDamageMultiplier());
            }
        }
    }

}

