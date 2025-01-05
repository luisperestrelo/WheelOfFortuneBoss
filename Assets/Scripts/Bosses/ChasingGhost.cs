using UnityEngine;

public class ChasingGhost : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float damage = 10f;
    private Transform player;

    public void Initialize(Transform player)
    {
        this.player = player;
    }

    private void Update()
    {
        if (player == null) return; 

        // Continuously track and move towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Destroy(gameObject); // Destroy ghost on impact
            }
        }
    }
} 