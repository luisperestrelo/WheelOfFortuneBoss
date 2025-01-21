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

    private PlayerCombat pc;

    public UnityEvent<float, float> OnHealthChanged;

    public UnityEvent OnDie;

    private bool isAlive = true;

    protected virtual void Awake()
    {
        pc = GetComponent<PlayerCombat>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged.Invoke(currentHealth, maxHealth);
        Debug.Log(gameObject.name + " has " + currentHealth + " health!");
    }

    // should be in PlayerHealth but I am keeping it here in case we add shields for enemies
    /// <returns>Whether or not HP was lost</returns>
    public virtual bool TakeDamage(float damageAmount)
    {
        if (pc != null && pc.HasShield)
        {
            pc.RemoveShield();
            damageSource.PlayOneShot(parrySfx);
            return false;
        }


        Debug.Log( gameObject.name + " is Taking damage: " + damageAmount);

        currentHealth -= damageAmount;
        OnHealthChanged.Invoke(currentHealth, maxHealth);

        //SFX
        if (damageAmount < hpHeavyDamageThreshold)
            damageSource.PlayOneShot(lightDamageSfx);
        else
            damageSource.PlayOneShot(heavyDamageSfx);

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
        currentHealth = maxHealth;
        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }
}