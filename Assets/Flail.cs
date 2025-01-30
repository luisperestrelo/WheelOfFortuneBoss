using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flail : MonoBehaviour
{
    
    [SerializeField] private GameObject flailVfxPrefab;
    [SerializeField] private GameObject crackVfxPrefab;
    [SerializeField] private Transform flailVfxSpawnPosition;
    [SerializeField] private Transform crackVfxSpawnPosition;

    [SerializeField] private PolygonCollider2D damageCollider;
    [SerializeField] private float damage;

    private void Awake()
    {
        damageCollider = GetComponent<PolygonCollider2D>();
        damageCollider.enabled = false;

    }

    public void Hit()
    {
        Instantiate(flailVfxPrefab, flailVfxSpawnPosition.position, Quaternion.identity);
        Instantiate(crackVfxPrefab, crackVfxSpawnPosition.position, Quaternion.identity);
        StartCoroutine(EnableDamageCollider()); // for damage
    }

    private IEnumerator EnableDamageCollider()
    {
        damageCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

}
