using UnityEngine;

[CreateAssetMenu(fileName = "FireballAttack", menuName = "Attacks/Fireball Attack")]
public class FireballAttack : BaseAttack
{
    [SerializeField] private FireballProjectile fireballPrefab;

    public override void PerformAttack(PlayerCombat playerCombat, float fireRate, PlayerStats playerStats, int projectileCount, float spreadAngle = 15f)
    {
        base.PerformAttack(playerCombat);

        float damageMultiplier = playerCombat.GetUniversalDamageMultiplier();

        if (Random.value < playerStats.CritChance)
        {
            damageMultiplier *= playerStats.CritMultiplier;
            Debug.Log("Fireball CRIT!");
        }

        //playerCombat.shootAudioSource.PlayOneShot(playerCombat.shootSfx); //TODO: Add fireball sfx
        //playerCombat.shootAudioSource.pitch = Random.Range(0.9f, 1.3f);

        Plane plane = new(Vector3.forward, playerCombat.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint;

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            hitPoint = ray.GetPoint(distance);

            Vector2 towardMouse = (hitPoint - playerCombat.transform.position).normalized;

            spreadAngle = Mathf.Clamp(spreadAngle, 0f, 180f);

            if (projectileCount > 1)
            {
                float mouseAngle = Mathf.Atan2(towardMouse.y, towardMouse.x) * Mathf.Rad2Deg;
                float startAngle = mouseAngle - spreadAngle / 2f;

                for (int i = 0; i < projectileCount; i++)
                {
                    float currentAngle = startAngle + spreadAngle / (projectileCount - 1) * i;

                    Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
                    Vector2 direction = rotation * Vector2.right;

                    FireballProjectile projectile = Instantiate(fireballPrefab, playerCombat.transform.position, Quaternion.identity);
                    projectile.SetDamage(BaseDamage * damageMultiplier);
                    projectile.SetVelocity(direction * ProjectileSpeed);
                }
            }
            else
            {
                // Handle cases where projectileCount is not greater than 1
                Vector2 direction = towardMouse; // Default direction

                FireballProjectile projectile = Instantiate(fireballPrefab, playerCombat.transform.position, Quaternion.identity);
                projectile.SetDamage(BaseDamage * damageMultiplier);
                projectile.SetVelocity(direction * ProjectileSpeed);
            }
        }

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(fireRate));
    }
}