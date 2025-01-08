using UnityEngine;

public class RangedMinion : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float shootingCooldown = 1f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint; 
    private Transform player;
    private float nextShootTime;
    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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

        // Rotate to face the player
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // Assuming the minion's sprite is facing up

        if (Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + shootingCooldown;
        }
    }

    //TODO also fix projectile logic to avoid Setters, but do that later
    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        EnemyProjectile proj = projectile.GetComponent<EnemyProjectile>();
        if (proj != null)
        {
            proj.SetDamage(damage);
            proj.SetVelocity(projectile.transform.up * projectileSpeed);
        }
    }
} 