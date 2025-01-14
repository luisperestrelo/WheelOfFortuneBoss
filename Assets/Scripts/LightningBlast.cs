using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LightningBlast : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private AudioClip strikeSfx;
    private PlayerStats playerStats;
    private PlayerCombat playerCombat;
    private AudioSource audioSource;

    private void Start()
    {
        Destroy(gameObject, lifetime);
        playerStats = FindObjectOfType<PlayerStats>();
        playerCombat = FindObjectOfType<PlayerCombat>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(strikeSfx);
    }

    // Could do this with animation triggers later, if we want
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
                float finalDamage = damage;

                finalDamage *= playerCombat.GetUniversalDamageMultiplier();

                if (Random.value < playerStats.CritChance)
                {
                    finalDamage *= playerStats.CritMultiplier;
                    Debug.Log("Lightning Blast CRIT!"); 
                    //TODO: Would be cool to have something to add more "oomph" to crits but maybe too much?
                }

                enemyHealth.TakeDamage(finalDamage);
            }
        }
    }
}

