using UnityEngine;

public class ChasingGhost : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float damage = 10f;
    private Transform player;
    private PlayerHealth playerHealth;
    


    public void Initialize(Transform player, float speed, float damage)
    {
        this.player = player;
        this.playerHealth = player.GetComponent<PlayerHealth>();
        this.speed = speed;
        this.damage = damage;
    }

    private void Update()
    {
        if (player == null) return;

        // Continuously track and move towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Destroy(gameObject); // Destroy ghost on impact
            }
        }
    }
}