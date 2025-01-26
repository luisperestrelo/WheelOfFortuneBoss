using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float shootCooldown = 0.2f;
    public float projectileSpeed = 20f;
    [SerializeField] private BaseProjectile defaultProjectilePrefab;

    [SerializeField] private AudioClip increasedDamageLayerSfx;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform visuals;

    private BaseProjectile projectilePrefab;

    public AudioSource shootAudioSource;
    private Coroutine damageCoroutine;
    private Coroutine fireRateCoroutine;

    public bool HasShield { get; private set; }
    private GameObject _activeShield;
    private ShieldArea _currentShieldArea;

    private bool canShoot = true;

    public BaseAttack CurrentAttack;
    public BaseAttack DefaultAttack;

    private PlayerStats playerStats;

    // Temporary multipliers for buffs
    private float temporaryDamageMultiplier = 1f;
    private float temporaryFireRateMultiplier = 1f;

    // FireballAttack (and other attacks) can call this if a crit occurs
    public UnityEvent OnCrit;  

    private void Awake()
    {
        if (OnCrit == null)
            OnCrit = new UnityEvent();
    }

    private void Start()
    {
        shootAudioSource = GetComponent<AudioSource>();
        projectilePrefab = defaultProjectilePrefab;
        playerStats = FindObjectOfType<PlayerStats>();
    }


    private void RotatePlayerTowardsMouse()
    {
        
        Plane plane = new(Vector3.forward,  transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint;
        float distance;
        
        if (plane.Raycast(ray, out distance))
        {
            hitPoint = ray.GetPoint(distance);
            Vector2 towardMouse = (hitPoint -  transform.position).normalized;
            float mouseAngle = Mathf.Atan2(towardMouse.y, towardMouse.x) * Mathf.Rad2Deg;
            visuals.rotation = Quaternion.Euler(0, 0, mouseAngle - 90f);
        }
    }
    
    private void Update()
    {
        RotatePlayerTowardsMouse();
        
        if (canShoot && Input.GetMouseButton(0))
        {
            if (CurrentAttack == null)
                CurrentAttack = DefaultAttack;
            playerAnimator.SetTrigger("Attack");

            // Calculate fire rate with temporary buff
            float fireRate = CurrentAttack.FireRate * playerStats.BaseFireRateMultiplier * temporaryFireRateMultiplier;
            
            int totalProjectiles = CalculateTotalProjectiles(CurrentAttack, shouldFanOut: true);

            CurrentAttack.PerformAttack(this, fireRate, playerStats, totalProjectiles);
            if (temporaryDamageMultiplier > 1)
                shootAudioSource.PlayOneShot(increasedDamageLayerSfx);
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
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(DamageMultiplierCoroutine(multiplier, duration));
    }

    public void IncreaseDamageMultiplierForDuration(float multiplier, float duration)
    {
        SetDamageMultiplierForDuration(temporaryDamageMultiplier * multiplier, duration);
    }

    private IEnumerator DamageMultiplierCoroutine(float multiplier, float duration)
    {
        temporaryDamageMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        temporaryDamageMultiplier = 1f;
        damageCoroutine = null;
    }

    // temporary fire rate buffs
    public void SetFireRateMultiplierForDuration(float multiplier, float duration)
    {
        if (fireRateCoroutine != null)
        {
            StopCoroutine(fireRateCoroutine);
        }
        fireRateCoroutine = StartCoroutine(FireRateMultiplierCoroutine(multiplier, duration));
    }

    public void IncreaseFireRateMultiplierForDuration(float multiplier, float duration)
    {
        SetFireRateMultiplierForDuration(temporaryFireRateMultiplier * multiplier, duration);
    }

    private IEnumerator FireRateMultiplierCoroutine(float multiplier, float duration)
    {
        temporaryFireRateMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        temporaryFireRateMultiplier = 1f;
        fireRateCoroutine = null;
    }

    public void ActivateShield(GameObject shieldPrefab, ShieldArea source)
    {
        HasShield = true;
        _currentShieldArea = source;
        //if (shieldPrefab == null) return;
        _activeShield = Instantiate(this.shieldPrefab, transform.position, Quaternion.identity, transform);
        StartCoroutine(RemoveShieldAfterDelay(source));
    }

    private IEnumerator RemoveShieldAfterDelay(ShieldArea source)
    {
        yield return new WaitForSeconds(0.5f);
        if (source == _currentShieldArea)
            RemoveShield();
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canShoot = true;
        Debug.Log("canShoot reset to true on scene load: " + scene.name);
    }

    public float GetUniversalDamageMultiplier()
    {
        //return playerStats.BaseDamageMultiplier * temporaryDamageMultiplier;
        return playerStats.GetAggregatedDamageMultiplier();
    }
    private int CalculateTotalProjectiles(BaseAttack attack, bool shouldFanOut = false)
    {
        return attack.ProjectileCount + Mathf.FloorToInt(playerStats.AdditionalProjectilesForAttacks);
    }

    public float GetTemporaryDamageMultiplier()
    {
        return temporaryDamageMultiplier;
    }

    public void SetTemporaryDamageMultiplier(float newMultiplier)
    {
        temporaryDamageMultiplier = newMultiplier;
        // If you want to handle stacking multiple buffs, you might do more logic here
    }

    public void NotifyCrit()
    {
        OnCrit.Invoke();
    }
}
