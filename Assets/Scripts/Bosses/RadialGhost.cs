using UnityEngine;

public class RadialGhost : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float damage = 10f;
    private CircularPath path;
    private Transform player;
    private float currentAngle;
    private int direction; // 1 for clockwise, -1 for counter-clockwise

    public void Initialize(CircularPath path, Transform player, int direction)
    {
        this.path = path;
        this.player = player;
        this.direction = direction;

        // Calculate initial angle based on player's position and direction
        Vector3 toPlayer = player.position - path.GetCenter();
        currentAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        currentAngle += direction > 0 ? -180f : 180f; // Start 180 degrees away

        UpdatePosition();
    }

    private void Update()
    {
        // Move along the path
        currentAngle += direction * speed * Time.deltaTime;

        // Keep angle within 0-360 range
        currentAngle = (currentAngle + 360f) % 360f;

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // Calculate new position based on current angle and path radius
        float x = path.GetCenter().x + path.GetRadius() * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = path.GetCenter().y + path.GetRadius() * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        transform.position = new Vector3(x, y, transform.position.z);
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
            // Them taking damage is handled in whatever script damages them.
        }
    }
} 