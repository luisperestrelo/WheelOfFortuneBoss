using UnityEngine;

[CreateAssetMenu(fileName = "WagerOfAeonsAttack", menuName = "Attacks/WagerOfAeons Attack")]
public class WagerOfAeonsAttack : BaseAttack
{
    [Header("Projectiles by Level (0 through 5)")]
    [SerializeField] private WagerOfAeonsProjectile[] projectilesByLevel; // 6 variants total
    private int currentLevel = 0;

    [Header("WagerOfAeons Config")]
    [Tooltip("How many seconds to wait before the 'level' upgrades by 1?")]
    [SerializeField] private float timeToUpgrade = 1f;

    [Tooltip("How many seconds between each Vulnerability application?")]
    [SerializeField] private float timeToApplyVulnerability = 1f;

    [Tooltip("Duration of each Vulnerability applied.")]
    [SerializeField] private float vulnerabilityDuration = 3f;

    [Tooltip("Multiplier step for each VulnerabilityBuff stack.")]
    [SerializeField] private float vulnerabilityMultiplier = 1.2f;

    [Header("Per-Level Damage")]
    [SerializeField] private float[] damagePerLevel = new float[6] { 10, 15, 20, 30, 40, 60 };

    public float TimeToUpgrade => timeToUpgrade;
    public float TimeToApplyVulnerability => timeToApplyVulnerability;
    public float VulnerabilityDuration => vulnerabilityDuration;
    public float VulnerabilityMultiplier => vulnerabilityMultiplier;

    public override void PerformAttack(PlayerCombat playerCombat, float fireRate, PlayerStats playerStats, int projectileCount, float spreadAngle)
    {
        base.PerformAttack(playerCombat, fireRate, playerStats, projectileCount, spreadAngle);

        if (currentLevel < 0 || currentLevel >= damagePerLevel.Length)
        {
            Debug.LogWarning("WagerOfAeonsAttack: currentLevel out of range for damagePerLevel!");
            return;
        }
        if (currentLevel >= projectilesByLevel.Length)
        {
            Debug.LogWarning("WagerOfAeonsAttack: currentLevel out of range for projectiles array!");
            return;
        }

        float baseDamageForLevel = damagePerLevel[currentLevel];
        WagerOfAeonsProjectile selectedProjectile = projectilesByLevel[currentLevel];

        if (selectedProjectile == null)
        {
            Debug.LogWarning($"WagerOfAeonsAttack: No projectile assigned for level {currentLevel}");
            return;
        }

        Plane plane = new Plane(Vector3.forward, playerCombat.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float distance))
        {
            Vector2 towardMouse = (ray.GetPoint(distance) - playerCombat.transform.position).normalized;
            spreadAngle = Mathf.Clamp(spreadAngle, 0f, 180f);

            float damageMultiplier = playerCombat.GetUniversalDamageMultiplier();
            if (Random.value < playerStats.GetAggregatedCritChance())
            {
                damageMultiplier *= playerStats.CritMultiplier;
                Debug.Log("WagerOfAeons CRIT!");
                playerCombat.NotifyCrit();
            }

            float finalDamage = baseDamageForLevel * damageMultiplier * playerStats.PositiveNegativeFieldsEffectivenessMultiplier;
            
            //TODO: Rotate the projectile , cba doing it right now
            if (projectileCount > 1)
            {
                float mouseAngle = Mathf.Atan2(towardMouse.y, towardMouse.x) * Mathf.Rad2Deg;
                float startAngle = mouseAngle - spreadAngle / 2f;

                for (int i = 0; i < projectileCount; i++)
                {
                    float currentAngle = startAngle + (spreadAngle / (projectileCount - 1)) * i;
                    Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
                    Vector2 direction = rotation * Vector2.right;

                    
                    //todo fix rotation
                    var projectile = Instantiate(selectedProjectile, playerCombat.transform.position, Quaternion.Euler(-45, 0, 0));
                    projectile.SetDamage(finalDamage);
                    projectile.SetVelocity(direction * ProjectileSpeed);
                    projectile.SetPoisonStats(
                        playerStats.PoisonChance,
                        playerStats.BasePoisonDamage * damageMultiplier * playerStats.PoisonDamageOverTimeMultiplier,
                        playerStats.BasePoisonDuration * playerStats.PoisonDurationMultiplier
                    );
                }
            }
            else
            {
                var projectile = Instantiate(selectedProjectile, playerCombat.transform.position, Quaternion.Euler(-45, 0, 0));
                projectile.SetDamage(finalDamage);
                projectile.SetVelocity(towardMouse * ProjectileSpeed);
                projectile.SetPoisonStats(
                    playerStats.PoisonChance,
                    playerStats.BasePoisonDamage * damageMultiplier * playerStats.PoisonDamageOverTimeMultiplier,
                    playerStats.BasePoisonDuration * playerStats.PoisonDurationMultiplier
                );
            }
        }

        playerCombat.StartCoroutine(playerCombat.ShootCooldownRoutine(fireRate));
    }

    // Called by the EffectHandler each X seconds
    public void IncrementAttackLevel()
    {
        if (currentLevel < projectilesByLevel.Length - 1)
        {
            currentLevel++;
            Debug.Log($"WagerOfAeonsAttack: Increased to level {currentLevel}.");
        }
    }

    public void ResetAttackLevel()
    {
        currentLevel = 0;
        Debug.Log("WagerOfAeonsAttack: Reset level to 0.");
    }
}