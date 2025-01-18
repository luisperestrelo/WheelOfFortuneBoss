using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RangedMinion : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float shootingCooldown = 1f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private SpriteRenderer visuals;
    [SerializeField] private float maxRotationDegrees = 30;

    [SerializeField] private AudioClip shootSfx;
    private AudioSource source;

    private Transform player;
    private float nextShootTime;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.85f, 1.15f);
    }

    // Constructor to initialize values
    public void Initialize(float projectileSpeed, float shootingCooldown, float damage)
    {
        this.projectileSpeed = projectileSpeed;
        this.shootingCooldown = shootingCooldown;
        this.damage = damage;
        nextShootTime = Time.time + shootingCooldown;
    }

    private void Update()
    {
        if (player == null) return;

        RotateTowardsPlayer();

        if (Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + shootingCooldown;
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.transform.rotation =
            Quaternion.Euler(0f, 0f, angle - 90f); // Assuming the minion's sprite is facing up

        // Some Minions might not have a dedicated visuals game object 
        var transformToRotate = visuals ? visuals.transform : transform;

        if (direction.x > 0)
        {
            angle = Mathf.Clamp(angle, -maxRotationDegrees, maxRotationDegrees);

            var scale = transformToRotate.localScale;
            if (scale.x < 0)
                transformToRotate.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        // flip X 
        else
        {
            angle = Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg;
            angle = -Mathf.Clamp(angle, -30, 30);

            var scale = transformToRotate.localScale;
            if (scale.x > 0)
                transformToRotate.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
        }

        transformToRotate.rotation = Quaternion.Euler(0, 0, angle);
    }

    //TODO also fix projectile logic to avoid Setters, but do that later
    private void Shoot()
    {
        source.pitch = Random.Range(0.85f, 1.15f);
        source.PlayOneShot(shootSfx);
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        EnemyProjectile proj = projectile.GetComponent<EnemyProjectile>();
        if (proj != null)
        {
            proj.SetDamage(damage);
            proj.SetVelocity(projectile.transform.up * projectileSpeed);
        }
    }
}