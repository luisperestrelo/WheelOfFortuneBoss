using UnityEngine;

public class LinearGhost : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 10f;
    private Transform player;
    private Vector3 direction;

    public void Initialize(Transform player)
    {
        this.player = player;
        direction = (player.position - transform.position).normalized;
        Destroy(gameObject, lifeTime); // TODO: A lot of our abilities should destroy when they are "outside of the camera", rather
        // than managing these timers which will eventually be a pain to manage. eg if I suddenly make this little guy MUCH slower
        //, then he would destroy himself way too early, so I'd have to come here and change the lifetime, etc.
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