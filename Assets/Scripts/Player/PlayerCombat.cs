using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float shootCooldown = 0.2f;
    public float projectileSpeed = 20f;
    [SerializeField] private BaseProjectile defaultProjectilePrefab;
    [SerializeField] private float globalDamageMultiplier = 1f;
    [SerializeField] private float baseDamageMultiplier = 1f;

    [SerializeField] private AudioClip shootSfx;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private Animator playerAnimator;

    private BaseProjectile projectilePrefab;

    public AudioSource shootAudioSource;
    private Coroutine damageCoroutine;

    public bool HasShield { get; private set; }
    private GameObject _activeShield;
    private ShieldArea _currentShieldArea;

    private bool canShoot = true;

    public BaseAttack CurrentAttack;
    public BaseAttack DefaultAttack;

    private PlayerStats playerStats;

    private void Start()
    {
        shootAudioSource = GetComponent<AudioSource>();
        projectilePrefab = defaultProjectilePrefab;
        playerStats = FindObjectOfType<PlayerStats>();
        baseDamageMultiplier = playerStats.BaseDamage;
    }

    private void Update()
    {

        if (canShoot && Input.GetMouseButton(0))
        {
            if (CurrentAttack == null)
                CurrentAttack = DefaultAttack;
            //CurrentAttack.BaseDamage = playerStats.BaseDamage; // uses the skill's base damage
            playerAnimator.SetTrigger("Attack");
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
            globalDamageMultiplier = baseDamageMultiplier;
        }

        damageCoroutine = StartCoroutine(DamageMultiplierCoroutine(multiplier, duration));
    }

    public void IncreaseDamageMultiplierForDuration(float multiplier, float duration) // this is multiplicative, not additive, I think?
    {
        SetDamageMultiplierForDuration(globalDamageMultiplier * multiplier, duration);
    }

    private IEnumerator DamageMultiplierCoroutine(float multiplier, float duration)
    {
        globalDamageMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        globalDamageMultiplier = baseDamageMultiplier;
        damageCoroutine = null;
    }

    public void ActivateShield(GameObject shieldPrefab, ShieldArea source) // not using the parameters
    {

        HasShield = true;
        _currentShieldArea = source;
        //if (shieldPrefab == null) return;
        _activeShield = Instantiate(this.shieldPrefab, transform.position, Quaternion.identity, transform);
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

    public float GetBaseDamageMultiplier()
    {
        return baseDamageMultiplier;
    }

    public void UpdateBaseDamageMultiplier(float newBaseMultiplier)
    {
        baseDamageMultiplier = newBaseMultiplier;
        globalDamageMultiplier = baseDamageMultiplier; //TODO: uhh
    }

    //Maybe a ghetto solution but if we went into a new scene and canShoot was false from before, it wouldnt work
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
}
