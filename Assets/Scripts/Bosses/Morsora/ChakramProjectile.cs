using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChakramProjectile : MonoBehaviour
{
    public float damage = 5f;
    public float damageCooldown = 1f;
    private float lastDamageTime = -Mathf.Infinity;

    public float stopDuration = 3f;
    public float duration = 2.5f;

    private bool isStopped = false;
    private CircularPath circularPath;

    private Coroutine damageOverTimeCoroutine;

    public float growthRate = 2f; // Units per second
    public float maxScale = 1f; // Maximum scale the chakram can reach

    private void Start()
    {
        circularPath = FindObjectOfType<CircularPath>();
        if (circularPath == null)
        {
            Debug.LogError("CircularPath not found in the scene!");
        }

        transform.localScale = new Vector3(0.1f, 0.1f, transform.localScale.z);

        Destroy(gameObject, duration);
    }

    private void Update()
    {
        if (!isStopped)
        {
            if (HasReachedCircularPath())
            {
                isStopped = true;
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero; // Stop the chakram
                }

                //StartCoroutine(StopAndDestroy());

            }
            else
            {
                Grow();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damageOverTimeCoroutine = StartCoroutine(DamageOverTime(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damageOverTimeCoroutine != null)
            {
                StopCoroutine(damageOverTimeCoroutine);
            }
        }
    }

    private IEnumerator DamageOverTime(GameObject player)
    {
        while (true)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(damage);
                lastDamageTime = Time.time;
            }
            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator StopAndDestroy()
    {
        isStopped = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Stop the chakram
        }

        yield return new WaitForSeconds(stopDuration);

        DestroyChakram();
    }

    public void DestroyChakram()
    {
        Destroy(gameObject);
    }

    private bool HasReachedCircularPath()
    {
        if (circularPath == null)
        {
            return false;
        }

        Vector2 center = circularPath.GetCenter();
        float radius = circularPath.GetRadius();
        Vector2 currentPosition = transform.position;

        // Check if the distance from the current position to the center is approximately equal to the radius
        return Mathf.Abs(Vector2.Distance(currentPosition, center) - radius) < 0.1f; // Using a small tolerance
    }

    private void Grow()
    {
        if (transform.localScale.x < maxScale)
        {
            float newScale = transform.localScale.x + growthRate * Time.deltaTime;
            transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
        }
    }
}
