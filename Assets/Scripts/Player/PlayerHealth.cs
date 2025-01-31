using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [SerializeField] private PlayerFlashFX playerFlashFX;
    [SerializeField] private PlayerSFX playerSFX;
    [SerializeField] private Animator animator;

    private PlayerStats playerStats;

    protected override void Awake()
    {
        base.Awake();
        playerStats = GetComponent<PlayerStats>();
    }

    protected override void Start()
    {
        SetMaxHealth(playerStats.MaxHealth);
        SetCurrentHealth(playerStats.MaxHealth);
        base.Start();
    }

    public override bool TakeDamage(float damageAmount, bool isDamageOverTime = false, bool isCrit = false)
    {
        StartCoroutine(HitpauseRoutine());
        if (!base.TakeDamage(damageAmount, isDamageOverTime, isCrit))
            return false;
        CameraMovement.instance.ShakeCamera(0.1f, 0.25f);

        playerFlashFX.PlayFlashFX();
        animator.SetTrigger("GetHit");

        /* deprecated
        //1/10 of the player's hp does nothing, 1/3 of the player's max hp is considered max intensity.
        if (damageAmount > (GetMaxHealth() / 10))
            MusicPlayer.instance.FilterMusic(Mathf.Max(1, damageAmount / (GetMaxHealth() * 3f))); */
        return true;
    }

    private IEnumerator HitpauseRoutine()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.1f);
        if (Time.timeScale == 0.1f)
            Time.timeScale = 1;
    }

    //Maybe a ghetto solution but it guarantees we start fight full health
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
        SetCurrentHealth(GetMaxHealth());
    }

    private void Update()
    {
        // Handle health regeneration in Update
        if (playerStats.HealthRegen > 0)
        {
            Regenerate(playerStats.HealthRegen * Time.deltaTime);
        }
    }

    // Separate method for healing
    public override void Heal(float healAmount)
    {
        base.Heal(healAmount);
    }

    // New method for health regeneration (separate from healing)
    private void Regenerate(float regenAmount)
    {

        if (currentHealth < maxHealth)
        {
            currentHealth += regenAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            OnHealthChanged.Invoke(GetCurrentHealth(), GetMaxHealth());
        }
    }

    protected override void Die()
    {
        OnDie.Invoke();

        //TODO: SFX/VFX etc.

        Debug.Log(gameObject.name + " died!");
        RunManager.Instance.RestartGame();
    }
}
