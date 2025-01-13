using UnityEngine;

[CreateAssetMenu(fileName = "FireballAttack", menuName = "Attacks/Fireball Attack")]
public class FireballAttack : BaseAttack
{
    [SerializeField] private FireballProjectile fireballPrefab;
    [SerializeField] private float fireballSpeed = 15f;

    public override void PerformAttack(PlayerCombat playerCombat, float fireRate)
    {
        base.PerformAttack(playerCombat);
        FireballProjectile projectile = Instantiate(fireballPrefab, playerCombat.transform.position, Quaternion.identity);

        // Get universal damage multiplier from PlayerCombat
        float damageMultiplier = playerCombat.GetUniversalDamageMultiplier();

        // Crit calculation
        if (Random.value < playerCombat.GetComponent<PlayerStats>().CritChance)
        {
            damageMultiplier *= playerCombat.GetComponent<PlayerStats>().CritMultiplier;
            Debug.Log("Fireball CRIT!");
        }

        //playerCombat.shootAudioSource.PlayOneShot(playerCombat.shootSfx); //TODO: Add fireball sfx
        //playerCombat.shootAudioSource.pitch = Random.Range(0.9f, 1.3f);
        projectile.SetDamage(BaseDamage * damageMultiplier);

        Plane plane = new(Vector3.forward, playerCombat.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint;

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            hitPoint = ray.GetPoint(distance);

            Vector2 towardMouse = (hitPoint - playerCombat.transform.position).normalized;
            projectile.SetVelocity(towardMouse * fireballSpeed);
        }

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(fireRate));
    }
}