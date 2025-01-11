using UnityEngine;

public class FireSlashAbility : MonoBehaviour
{
    [SerializeField] private GameObject purpleSlashPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float slashSpeed = 20f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void FireSlash()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        GameObject purpleSlash = Instantiate(purpleSlashPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        purpleSlash.transform.rotation = Quaternion.Euler(0, 0, angle);
        purpleSlash.GetComponent<Rigidbody2D>().velocity = direction * slashSpeed;
    }
} 