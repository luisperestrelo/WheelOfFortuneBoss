using UnityEngine;

[CreateAssetMenu(fileName = "FireballAttack", menuName = "Attacks/Fireball Attack")]
public class FireballAttack : BaseAttack
{
    [SerializeField] private FireballProjectile fireballPrefab;
    [SerializeField] private float fireballSpeed = 15f;

    public override void PerformAttack(PlayerCombat playerCombat)
    {
        base.PerformAttack(playerCombat);
        FireballProjectile projectile = Instantiate(fireballPrefab, playerCombat.transform.position, Quaternion.identity);
        projectile.SetDamage(BaseDamage * playerCombat.GetGlobalDamageMultiplier());

        //playerCombat.shootAudioSource.PlayOneShot(playerCombat.shootSfx); //TODO: Add fireball sfx
        //playerCombat.shootAudioSource.pitch = Random.Range(0.9f, 1.3f);

        //Cast a ray from the camera onto a plane (ground level).
        Plane plane = new(Vector3.forward, playerCombat.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint;

        //Aim where the ray intersects with the plane.
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            hitPoint = ray.GetPoint(distance);

            Vector2 towardMouse = (hitPoint - playerCombat.transform.position).normalized;
            projectile.SetVelocity(towardMouse * fireballSpeed);
        }

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(FireRate));
    }
} 