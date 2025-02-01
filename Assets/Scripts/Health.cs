using DamageNumbersPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHealth; // tbh, should be private but I like being able to see it in the inspector without going Debug mode


    [Tooltip("How much damage should be considered 'heavy damage' by the VFX and SFX")]
    [SerializeField] private int hpHeavyDamageThreshold = 25;
    [SerializeField] private AudioClip lightDamageSfx;
    [SerializeField] private AudioClip heavyDamageSfx;
    [SerializeField] protected AudioClip parrySfx;
    [SerializeField] private AudioClip healSfx;
    [SerializeField] protected AudioSource damageSource;

    [Header("Damage Numbers")]
    [Tooltip("Popup for hits below 125 damage (lightning blast base damage)")]
    public DamageNumber basicPopupPrefab;
    [Tooltip("Popup for hits above 125 damage (lightning blast base damage)")]
    public DamageNumber bigHitPopupPrefab; // Minimum damage = lightning blast dmg
                                           // the idea is that slower, but chunkier attacks

                                           // use this one instead
    [Tooltip("Popup for critical hits below 248 damage (lightning blast crit min damage)")]
    public DamageNumber criticalHitPopupPrefab;
    [Tooltip("Popup for critical hits above 248 damage (lightning blast crit min damage)")]
    public DamageNumber bigHitCriticalPopupPrefab;
    [Tooltip("Popup for healing")]
    public DamageNumber healPopupPrefab;


    [Header("Damage Numbers Offsets")]
    [Header("Adjust for each different enemy")]
    public Vector3 basicPopupOffset = new Vector3(0, 2.5f, -1);
    public Vector3 bigHitPopupOffset = new Vector3(1.5f, 1.5f, -1);
    public Vector3 criticalHitPopupOffset = new Vector3(0, 2f, -1);
    public Vector3 bigHitCriticalPopupOffset = new Vector3(0, 2f, -1);

    public Vector3 healPopupOffset = new Vector3(0, 0.5f, -1);



    private PlayerCombat pc;

    private Stats stats;

    public UnityEvent<float, float> OnHealthChanged;

    public UnityEvent OnDie;

    private bool isAlive = true;

    private DamageFlash damageFlash;

    protected virtual void Awake()
    {
        pc = GetComponent<PlayerCombat>();
        stats = GetComponent<Stats>();
        damageFlash = GetComponent<DamageFlash>();
    }

    protected virtual void Start()
    {
        isAlive = true;
        currentHealth = maxHealth;
        OnHealthChanged.Invoke(currentHealth, maxHealth);
        Debug.Log(gameObject.name + " has " + currentHealth + " health!");
    }

    // should be in PlayerHealth but I am keeping it here in case we add shields for enemies
    /// <returns>Whether or not HP was lost</returns>
    public virtual bool TakeDamage(float damageAmount, bool isDamageOverTime = false, bool isCrit = false)
    {
        if (pc != null && pc.HasShield)
        {
            pc.RemoveShield();
            damageSource.PlayOneShot(parrySfx);
            return false;
        }

        float damageTakenMultiplier = stats.GetAggregatedDamageTakenMultiplier();
        damageAmount *= damageTakenMultiplier;

        Debug.Log(gameObject.name + " is Taking damage: " + damageAmount);

        currentHealth -= damageAmount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        Debug.Log("isDamageOverTime: " + isDamageOverTime);
        Debug.Log("isCrit in Health script: " + isCrit);
        //SFX + VFX
        if (!isDamageOverTime)
        {
            if(damageFlash)
                damageFlash.Play();
            if (damageAmount < hpHeavyDamageThreshold)
                damageSource.clip = lightDamageSfx;
            else
                damageSource.clip = heavyDamageSfx;
            damageSource.Play();

            if (isCrit)
            {
                if (damageAmount < 248f)
                { // lightning blast crit min damage
                    if (criticalHitPopupPrefab != null)
                    {
                        DamageNumber newPopup = criticalHitPopupPrefab.Spawn(transform.position + criticalHitPopupOffset, damageAmount);
                        newPopup.SetFollowedTarget(transform);
                    }
                }
                else
                {
                    if (bigHitCriticalPopupPrefab != null)
                    {
                        DamageNumber newPopup = bigHitCriticalPopupPrefab.Spawn(transform.position + bigHitCriticalPopupOffset, damageAmount);
                        newPopup.SetFollowedTarget(transform);
                    }
                }
            }
            else if (damageAmount < 124f) //lightning blast min damage

            {
                if (basicPopupPrefab != null)
                {
                    DamageNumber newPopup = basicPopupPrefab.Spawn(transform.position + basicPopupOffset, damageAmount);
                    newPopup.SetFollowedTarget(transform);
                }
            }
            else
            {
                if (bigHitPopupPrefab != null)
                {
                    DamageNumber newPopup = bigHitPopupPrefab.Spawn(transform.position + bigHitPopupOffset, damageAmount);
                    newPopup.SetFollowedTarget(transform);
                }

            }



        }

        if (currentHealth <= 0 && isAlive)
        {
            isAlive = false;
            Die();
        }

        Debug.Log($"{gameObject.name} took {damageAmount} damage!");
        return true;
    }

    public virtual void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHealthChanged.Invoke(currentHealth, maxHealth);
        damageSource.PlayOneShot(healSfx);
        if (healPopupPrefab != null)
        {
            Vector3 popupOffset = new Vector3(0, 0.5f, -1);
            DamageNumber newPopup = healPopupPrefab.Spawn(transform.position + popupOffset, healAmount);
            newPopup.SetScale(1.5f);
            newPopup.SetFollowedTarget(transform);
        }

        Debug.Log(gameObject.name + " healed " + healAmount + " health! Current health: " + currentHealth);

    }

    protected virtual void Die()
    {
        OnDie.Invoke();

        //TODO: SFX/VFX etc.

        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject);
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        float oldMaxHealth = maxHealth;
        maxHealth = newMaxHealth;
        currentHealth = currentHealth + (newMaxHealth - oldMaxHealth);
        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }

    public void IncreaseMaxHealth(float increaseAmount)
    {
        maxHealth += increaseAmount;
        currentHealth += increaseAmount;
        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    protected void SetCurrentHealth(float newHealth)
    {
        currentHealth = newHealth;
        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }

    public void SetHealthToFull()
    {
        Debug.Log("Setting health to full");
        currentHealth = maxHealth;
        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }
}