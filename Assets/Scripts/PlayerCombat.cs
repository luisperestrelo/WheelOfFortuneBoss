using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float defaultDamage = 10f;
    [SerializeField] private float shootCooldown = 0.2f;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private Projectile _defaultProjectilePrefab;
    [SerializeField] private float globalDamageMultiplier = 1f; // we need a better solution later

    private Projectile projectilePrefab;



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
        yield return new WaitForSeconds(duration);
        currentDamage = defaultDamage;
        damageCoroutine = null;
    }

    public void ShootProjectile()
    {
        Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.SetDamage(currentDamage);

        //Cast a ray from the camera onto a plane (ground level).
        Plane plane = new(Vector3.forward, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint;

        //Aim where the ray intersects with the plane.
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            hitPoint = ray.GetPoint(distance);

            Vector2 towardMouse = (hitPoint - transform.position).normalized;
            projectile.SetVelocity(towardMouse * projectileSpeed);
        }

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

    public float GetGlobalDamageMultiplier()
    {
        return globalDamageMultiplier;
    }

    public void SetGlobalDamageMultiplier(float multiplier)
    {
        globalDamageMultiplier = multiplier;
    }
}
