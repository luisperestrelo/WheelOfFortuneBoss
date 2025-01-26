using UnityEngine;

[CreateAssetMenu(fileName = "FanOfKnivesAttack", menuName = "Attacks/Fan of Knives Attack")]
public class FanOfKnivesAttack : BaseAttack
{
    [SerializeField] private KnifeProjectile knifePrefab;
    /*     [SerializeField] private int numberOfKnives = 3; */
    [SerializeField] private float spreadAngle = 10f;

    public override void PerformAttack(PlayerCombat playerCombat, float fireRate, PlayerStats playerStats, int projectileCount, float spreadAngle)
    {
        base.PerformAttack(playerCombat);
        Plane plane = new(Vector3.forward, playerCombat.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint;
        float distance;
        Vector2 towardMouse = Vector2.right; // Default if raycast fails

        spreadAngle = this.spreadAngle;
        if (plane.Raycast(ray, out distance))
        {
            hitPoint = ray.GetPoint(distance);
            towardMouse = (hitPoint - playerCombat.transform.position).normalized;
        }

        spreadAngle = Mathf.Clamp(spreadAngle, 0f, 180f);

        if (projectileCount > 1)
        {
            float mouseAngle = Mathf.Atan2(towardMouse.y, towardMouse.x) * Mathf.Rad2Deg;
            float angleStep = spreadAngle / (projectileCount - 1);
            float startAngle = mouseAngle - spreadAngle / 2f;

            for (int i = 0; i < projectileCount; i++)
            {
                float currentAngle = startAngle + angleStep * i;

                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
                Vector2 direction = rotation * Vector2.right;

                KnifeProjectile knife = Instantiate(knifePrefab, playerCombat.transform.position, Quaternion.identity);

                float damageMultiplier = playerCombat.GetUniversalDamageMultiplier();

                if (Random.value < playerStats.GetAggregatedCritChance())
                {
                    damageMultiplier *= playerStats.CritMultiplier;
                    Debug.Log("Knife CRIT!");
                    playerCombat.NotifyCrit();
                }

                knife.SetDamage(BaseDamage * damageMultiplier);
                knife.SetVelocity(direction * ProjectileSpeed);
                knife.SetPoisonStats(playerStats.PoisonChance, playerStats.BasePoisonDamage * damageMultiplier * playerStats.PoisonDamageOverTimeMultiplier, playerStats.BasePoisonDuration);

            }
        }
        else
        {
            // Handle cases where projectileCount is not greater than 1
            Vector2 direction = towardMouse; // Default direction

            KnifeProjectile knife = Instantiate(knifePrefab, playerCombat.transform.position, Quaternion.identity);

            float damageMultiplier = playerCombat.GetUniversalDamageMultiplier();

            if (Random.value < playerStats.GetAggregatedCritChance())
            {
                damageMultiplier *= playerStats.CritMultiplier;
                Debug.Log("Knife CRIT!");
                playerCombat.NotifyCrit();
            }

            knife.SetDamage(BaseDamage * damageMultiplier);
            knife.SetVelocity(direction * ProjectileSpeed);
            knife.SetPoisonStats(playerStats.PoisonChance, playerStats.BasePoisonDamage * damageMultiplier * playerStats.PoisonDamageOverTimeMultiplier, playerStats.BasePoisonDuration * playerStats.PoisonDurationMultiplier);
        }

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(fireRate));
    }
}