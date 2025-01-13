using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttack", menuName = "Attacks/Basic Attack")]
public class BasicAttack : BaseAttack
{
    [SerializeField] private BaseProjectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 20f;

    public override void PerformAttack(PlayerCombat playerCombat, float fireRate)
    {
        base.PerformAttack(playerCombat);
        BaseProjectile projectile = Instantiate(projectilePrefab, playerCombat.transform.position, Quaternion.identity);

        // Get combined damage multiplier from PlayerCombat
        float damageMultiplier = playerCombat.GetUniversalDamageMultiplier();

        // Crit calculation
        if (Random.value < playerCombat.GetComponent<PlayerStats>().CritChance)
        {
            damageMultiplier *= playerCombat.GetComponent<PlayerStats>().CritMultiplier;
            Debug.Log("Basic Attack CRIT!");
        }

        projectile.SetDamage(BaseDamage * damageMultiplier);

        Plane plane = new(Vector3.forward, playerCombat.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint;

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            hitPoint = ray.GetPoint(distance);

            Vector2 towardMouse = (hitPoint - playerCombat.transform.position).normalized;
            projectile.SetVelocity(towardMouse * projectileSpeed);
        }

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(fireRate));
    }
} 