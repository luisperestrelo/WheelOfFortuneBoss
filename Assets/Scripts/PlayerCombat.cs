using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float shootCooldown = 0.2f;
    public float projectileSpeed = 20f;
    [SerializeField] private BaseProjectile defaultProjectilePrefab;
    [SerializeField] private float globalDamageMultiplier = 1f;

    [SerializeField] private AudioClip shootSfx;

    private BaseProjectile projectilePrefab;

    public AudioSource shootAudioSource;
    private Coroutine damageCoroutine;

    public bool HasShield { get; private set; }
    private GameObject _activeShield;
    private ShieldArea _currentShieldArea;

    private bool canShoot = true;

    public BaseAttack CurrentAttack;
    public BaseAttack DefaultAttack;

    private void Start()
    {
        shootAudioSource = GetComponent<AudioSource>();
        projectilePrefab = defaultProjectilePrefab;
        globalDamageMultiplier = 1f;
    }

    private void Update()
    {
        if (canShoot && Input.GetMouseButton(0))
        {
            if (CurrentAttack == null)
                CurrentAttack = DefaultAttack;
            CurrentAttack.PerformAttack(this);
        }
    }

    public IEnumerator ShootCooldownRoutine(float cooldown)
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
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
        globalDamageMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        globalDamageMultiplier = 1f;
        damageCoroutine = null;
    }

    public void ActivateShield(GameObject shieldPrefab, ShieldArea source)
    {

        HasShield = true;
        _currentShieldArea = source;
        if (shieldPrefab == null) return;
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
