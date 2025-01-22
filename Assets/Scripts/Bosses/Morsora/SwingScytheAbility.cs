using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Triggered by animation event
public class SwingScytheAbility : MonoBehaviour
{
    [SerializeField] private GameObject darkSlashPrefab;
    [SerializeField] private GameObject lightSlashPrefab;
    [SerializeField] private Transform darkSlashSpawnPoint;
    [SerializeField] private Transform lightSlashSpawnPoint;
    [SerializeField] private float damage = 10f;
    [SerializeField] private Vector2 darkSlashDamageSize = new Vector2(23.21f, 6.5f);
    [SerializeField] private Vector2 lightSlashDamageSize = new Vector2(23.8f, 8.24f);
    [SerializeField] private Vector2 darkSlashOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector2 lightSlashOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private LayerMask playerLayer;

    /*     private void Update()
        {

        }
        private void SwingScythe(bool isDarkSlash)
        {
            if (isDarkSlash)
            {
                Instantiate(darkSlashPrefab, darkSlashSpawnPoint.position, Quaternion.identity);
            }
            else
            {
                GameObject lightSlash = Instantiate(lightSlashPrefab, lightSlashSpawnPoint.position, Quaternion.identity);
                lightSlash.transform.rotation = Quaternion.Euler(0, 0, 180);

            }
        } */

    private void SwingLightScythe()
    {
        GameObject lightSlash = Instantiate(lightSlashPrefab, lightSlashSpawnPoint.position, Quaternion.identity); // Is just a visual now
        lightSlash.transform.rotation = Quaternion.Euler(0, 0, 180);
        DealDamage(lightSlashSpawnPoint.position, lightSlashDamageSize + lightSlashOffset);
    }

    private void SwingDarkScythe()
    {
        Instantiate(darkSlashPrefab, darkSlashSpawnPoint.position, Quaternion.identity); // Is just a visual now
        DealDamage(darkSlashSpawnPoint.position, darkSlashDamageSize + darkSlashOffset);
    }

    private void DealDamage(Vector2 center, Vector2 damageSize)
    {
        Collider2D hitPlayer = Physics2D.OverlapBox(center, damageSize, 0, playerLayer);
        if (hitPlayer != null && hitPlayer.TryGetComponent<PlayerHealth>(out var health))
        {
            health.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the dark slash damage area
        Gizmos.color = new Color(0.5f, 0f, 0.5f); // Dark purple
        Gizmos.DrawWireCube(darkSlashSpawnPoint.position + (Vector3)darkSlashOffset, darkSlashDamageSize);

        // Draw the light slash damage area
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(lightSlashSpawnPoint.position + (Vector3)lightSlashOffset, lightSlashDamageSize);
    }
}
