using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnce : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    
    private bool hasDamaged = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDamaged) return;

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            hasDamaged = true;
        }
    }
}
