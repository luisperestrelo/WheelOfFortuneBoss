using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttack", menuName = "Attacks/Basic Attack")]
public class BasicAttack : BaseAttack
{
    [SerializeField] private BaseProjectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private AudioClip shootSfx;

    public override void PerformAttack(PlayerCombat playerCombat)
    {
        BaseProjectile projectile = Instantiate(projectilePrefab, playerCombat.transform.position, Quaternion.identity);
        projectile.SetDamage(BaseDamage * playerCombat.GetGlobalDamageMultiplier());

        playerCombat.shootAudioSource.PlayOneShot(shootSfx);
        playerCombat.shootAudioSource.pitch = Random.Range(0.9f, 1.3f);

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
            projectile.SetVelocity(towardMouse * projectileSpeed);
        }

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(FireRate));
    }
} 