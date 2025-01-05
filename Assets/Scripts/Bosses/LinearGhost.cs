using UnityEngine;

public class LinearGhost : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 10f;
    private Transform player;
    private Vector3 direction;

    public void Initialize(Transform player)
    {
        this.player = player;
        direction = (player.position - transform.position).normalized;
    }

    private void Update()
    {
        // Move towards the player
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
                Destroy(gameObject);
            }
        }
    }
}