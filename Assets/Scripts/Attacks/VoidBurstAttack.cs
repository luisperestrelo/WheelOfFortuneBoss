using UnityEngine;

[CreateAssetMenu(fileName = "VoidBurstAttack", menuName = "Attacks/Void Burst Attack")]
public class VoidBurstAttack : BaseAttack
{
    [SerializeField] private InstantDamageDealer voidBurstPrefab;
    //[SerializeField] private float voidBurstSpeed = 15f;  // not used since its not a projectile

    public override void PerformAttack(PlayerCombat playerCombat, float fireRate, PlayerStats playerStats)
    {
        base.PerformAttack(playerCombat);
        InstantDamageDealer damageDealer = Instantiate(voidBurstPrefab, playerCombat.transform.position, Quaternion.identity);

        // Get universal damage multiplier from PlayerCombat
        float damageMultiplier = playerCombat.GetUniversalDamageMultiplier() * playerStats.PositiveNegativeFieldsEffectivenessMultiplier;

        // Crit calculation
        if (Random.value < playerStats.CritChance)
        {
            damageMultiplier *= playerStats.CritMultiplier;
            Debug.Log("Void Burst CRIT!");
        }
        //playerCombat.shootAudioSource.PlayOneShot(playerCombat.shootSfx); //TODO: Add void burst sfx
        //playerCombat.shootAudioSource.pitch = Random.Range(0.9f, 1.3f);

        damageDealer.SetDamage(BaseDamage * damageMultiplier);

        Plane plane = new(Vector3.forward, playerCombat.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint;

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            hitPoint = ray.GetPoint(distance);

            Vector2 towardMouse = (hitPoint - playerCombat.transform.position).normalized;
            //rotate the damage dealer to face the mouse
            damageDealer.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(towardMouse.y, towardMouse.x) * Mathf.Rad2Deg);
            damageDealer.transform.Rotate(0, 0, 90);
            damageDealer.transform.position = playerCombat.transform.position + (Vector3)towardMouse * 6.2f;
        }

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(fireRate));
    }
}