using UnityEngine;

[CreateAssetMenu(fileName = "FanOfKnivesAttack", menuName = "Attacks/Fan of Knives Attack")]
public class FanOfKnivesAttack : BaseAttack
{
    [SerializeField] private KnifeProjectile knifePrefab;
    [SerializeField] private int numberOfKnives = 3;
    [SerializeField] private float spreadAngle = 30f;

    public override void PerformAttack(PlayerCombat playerCombat, float fireRate, PlayerStats playerStats)
    {
        base.PerformAttack(playerCombat);
        Plane plane = new(Vector3.forward, playerCombat.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint;
        float distance;
        Vector2 towardMouse = Vector2.right; // Default if raycast fails

        if (plane.Raycast(ray, out distance))
        {
            hitPoint = ray.GetPoint(distance);
            towardMouse = (hitPoint - playerCombat.transform.position).normalized;
        }

        float mouseAngle = Mathf.Atan2(towardMouse.y, towardMouse.x) * Mathf.Rad2Deg;

        float angleStep = spreadAngle / (numberOfKnives - 1);
        float startAngle = mouseAngle - spreadAngle / 2f;

        for (int i = 0; i < numberOfKnives; i++)
        {
            float currentAngle = startAngle + angleStep * i;

            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            Vector2 direction = rotation * Vector2.right;

            KnifeProjectile knife = Instantiate(knifePrefab, playerCombat.transform.position, Quaternion.identity);

            // Get universal damage multiplier from PlayerCombat
            float damageMultiplier = playerCombat.GetUniversalDamageMultiplier();

            // Crit calculation
            if (Random.value < playerStats.CritChance)
            {
                damageMultiplier *= playerStats.CritMultiplier;
                Debug.Log("Knife CRIT!");
            }

            knife.SetDamage(BaseDamage * damageMultiplier);
            knife.SetVelocity(direction * playerCombat.projectileSpeed);
        }

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(fireRate));
    }
}