using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float defaultDamage = 10f,
        shootCooldown = 0.2f,
        projectileSpeed = 20f;
    private Projectile projectilePrefab;
    [SerializeField] private Projectile _defaultProjectilePrefab;
    [SerializeField] private LayerMask wheelLayerMask;

    public Health health;
    

    private float currentDamage;
    private Coroutine damageCoroutine;

    public bool HasShield { get; private set; }
    private GameObject _activeShield;
    private ShieldArea _currentShieldArea;

    private bool canShoot = true;

    private void Start()
    {
        projectilePrefab = _defaultProjectilePrefab;
        currentDamage = defaultDamage;
    }

    private void Update()
    {
        if (canShoot && Input.GetMouseButton(0))
        {
            ShootProjectile();
        }
        if (GetWheelArea() != null)
            GetWheelArea().OnUpdate(this);
    }

    /// <returns>Whichever wheel area the player is currently on (null if none)</returns>
    private WheelArea GetWheelArea()
    {
        return Physics2D.OverlapCircle(transform.position, 0.1f, wheelLayerMask)?.GetComponent<WheelArea>();
    }

    private IEnumerator ShootCooldownRoutine()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    public void SetDamageMultiplierForDuration(float multiplier, float duration)
    {
        // If there's already a damage-coroutine running, stop it so durations don't overlap unpredictably.
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }

        damageCoroutine = StartCoroutine(DamageMultiplierCoroutine(multiplier, duration));
    }

    private IEnumerator DamageMultiplierCoroutine(float multiplier, float duration)
    {
        currentDamage = defaultDamage;
        currentDamage *= multiplier;       // Apply multiplier
        Debug.Log("Damage set to: " + currentDamage + " for " + duration + " seconds");
        yield return new WaitForSeconds(duration);  
        currentDamage = defaultDamage;     
        Debug.Log("Damage reset to: " + currentDamage);
        damageCoroutine = null;
    }

    public void ShootProjectile()
    {
        Projectile preprojectile;
        if (GetWheelArea() != null && GetWheelArea().effect != null)
            preprojectile = GetWheelArea().effect.DecorateProjectile(projectilePrefab);
        else
            preprojectile = projectilePrefab;

        Projectile projectile = Instantiate(preprojectile, transform.position, Quaternion.identity);
        projectile.SetDamage(currentDamage);

        Vector2 towardMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        projectile.SetVelocity(towardMouse * projectileSpeed);

        StartCoroutine(ShootCooldownRoutine());
    }
    
    /// <summary>
    /// Changes what type of projectile the player is currently firing.
    /// </summary>
    /// <param name="prefab">The new projecitle. Use null to reset to default.</param>
    public void SetProjectileType(Projectile prefab)
    {
        if (prefab != null)
            projectilePrefab = prefab;
        else
            projectilePrefab = _defaultProjectilePrefab;
    }

    public void ActivateShield(GameObject shieldPrefab, ShieldArea source)
    {
        if (shieldPrefab == null) return;

        HasShield = true;
        _currentShieldArea = source;
        _activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
    }

    public void RemoveShield()
    {
        HasShield = false;

        if (_activeShield != null)
        {
            Destroy(_activeShield);
            _activeShield = null;
        }

        _currentShieldArea = null;
    }
}
