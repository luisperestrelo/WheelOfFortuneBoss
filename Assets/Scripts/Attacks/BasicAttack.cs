using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttack", menuName = "Attacks/Basic Attack")]
public class BasicAttack : BaseAttack
{
    [SerializeField] private BaseProjectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 20f;
  //  [SerializeField] private AudioClip shootSfx;
//
    public override void PerformAttack(PlayerCombat playerCombat)
    {
        base.PerformAttack(playerCombat);
        BaseProjectile projectile = Instantiate(projectilePrefab, playerCombat.transform.position, Quaternion.identity);
        Debug.Log("BaseDamage: " + BaseDamage + " playerCombat.GetGlobalDamageMultiplier(): " + playerCombat.GetGlobalDamageMultiplier());
        projectile.SetDamage(BaseDamage * playerCombat.GetGlobalDamageMultiplier());


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

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(FireRate));
    }
} 