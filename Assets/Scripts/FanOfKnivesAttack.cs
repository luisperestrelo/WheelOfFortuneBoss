using UnityEngine;

[CreateAssetMenu(fileName = "FanOfKnivesAttack", menuName = "Attacks/Fan of Knives Attack")]
public class FanOfKnivesAttack : BaseAttack
{
    [SerializeField] private KnifeProjectile knifePrefab;
    [SerializeField] private int numberOfKnives = 3;
    [SerializeField] private float spreadAngle = 30f;

    public override void PerformAttack(PlayerCombat playerCombat)
    {
        // 1. Calculate the direction towards the mouse
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

        // 2. Calculate the angle of the mouse direction
        float mouseAngle = Mathf.Atan2(towardMouse.y, towardMouse.x) * Mathf.Rad2Deg;

        // 3. Calculate the angle step and starting angle
        float angleStep = spreadAngle / (numberOfKnives - 1);
        float startAngle = mouseAngle - spreadAngle / 2f;

        // 4. Instantiate knives with adjusted angles
        for (int i = 0; i < numberOfKnives; i++)
        {
            float currentAngle = startAngle + angleStep * i;

            // Calculate direction for each knife
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            Vector2 direction = rotation * Vector2.right;

            // Instantiate and set up the knife (using Quaternion.identity)
            KnifeProjectile knife = Instantiate(knifePrefab, playerCombat.transform.position, Quaternion.identity);
            knife.SetDamage(BaseDamage * playerCombat.GetGlobalDamageMultiplier());
            knife.SetVelocity(direction * playerCombat.projectileSpeed);
        }

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(FireRate));
    }
}