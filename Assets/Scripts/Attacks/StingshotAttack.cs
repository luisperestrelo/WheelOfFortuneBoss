using UnityEngine;

[CreateAssetMenu(fileName = "StingshotAttack", menuName = "Attacks/Stingshot Attack")]
public class StingshotAttack : BaseAttack
{
    [SerializeField] private StingshotProjectile stingshotPrefab;

    public override void PerformAttack(PlayerCombat playerCombat, float fireRate, PlayerStats playerStats, int projectileCount, float spreadAngle = 10f)
    {
        base.PerformAttack(playerCombat);

        bool isCrit = false;

        float damageMultiplier = playerCombat.GetUniversalDamageMultiplier();

        if (Random.value < playerStats.GetAggregatedCritChance())
        {
            damageMultiplier *= playerStats.CritMultiplier;
            Debug.Log("Stingshot CRIT!"); // TODO: This needs to be moved to the projectile itself...
            playerCombat.NotifyCrit();
            isCrit = true;

        }

        Debug.Log(spreadAngle);

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

                    StingshotProjectile projectile = Instantiate(stingshotPrefab, playerCombat.transform.position, Quaternion.identity);
                    projectile.SetDamage(BaseDamage * damageMultiplier);
                    projectile.SetVelocity(direction * ProjectileSpeed);
                    //scales with poison chance in addition to normal scaling

                    projectile.SetPoisonStats(1f, playerStats.BasePoisonDamage * damageMultiplier * playerStats.PoisonDamageOverTimeMultiplier * (1+ playerStats.PoisonChance), playerStats.BasePoisonDuration * playerStats.PoisonDurationMultiplier);
                    projectile.SetCrit(isCrit);
                }
            }
            else
            {
                // Handle cases where projectileCount is not greater than 1
                Vector2 direction = towardMouse; // Default direction

                StingshotProjectile projectile = Instantiate(stingshotPrefab, playerCombat.transform.position, Quaternion.identity);

                projectile.SetDamage(BaseDamage * damageMultiplier);
                projectile.SetVelocity(direction * ProjectileSpeed);
                //scales with poison chance in addition to normal scaling
                projectile.SetPoisonStats(1f, playerStats.BasePoisonDamage * damageMultiplier * playerStats.PoisonDamageOverTimeMultiplier * (1+ playerStats.PoisonChance), playerStats.BasePoisonDuration * playerStats.PoisonDurationMultiplier);
                projectile.SetCrit(isCrit);
            }
        }

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(fireRate));
    }
}