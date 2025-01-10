using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth; // tbh, should be private but I like being able to see it in the inspector without going Debug mode

    [Tooltip("How much damage should be considered 'heavy damage' by the VFX and SFX")]
    [SerializeField] private int hpHeavyDamageThreshold = 25;
    [SerializeField] private AudioClip lightDamageSfx;
    [SerializeField] private AudioClip heavyDamageSfx;
    [SerializeField] private AudioClip parrySfx;

    private PlayerCombat pc;

    public UnityEvent<float, float> OnHealthChanged;

    public UnityEvent OnDie;

    protected virtual void Awake()
    {
        pc = GetComponent<PlayerCombat>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }

    // should be in PlayerHealth but I am keeping it here in case we add shields for enemies
    public virtual void TakeDamage(float damageAmount)
    {
        if (pc != null && pc.HasShield)
        {
            pc.RemoveShield();
            SFXPool.instance.PlaySound(parrySfx);
            return;
        }


        Debug.Log( gameObject.name + " is Taking damage: " + damageAmount);

        currentHealth -= damageAmount;
        OnHealthChanged.Invoke(currentHealth, maxHealth);

        //SFX
        if (damageAmount < hpHeavyDamageThreshold)
            SFXPool.instance.PlaySound(lightDamageSfx);
        else
            SFXPool.instance.PlaySound(heavyDamageSfx);

        if (currentHealth <= 0)
        {
            Die();
        }

        Debug.Log($"{gameObject.name} took {damageAmount} damage!");
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHealthChanged.Invoke(currentHealth, maxHealth);
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
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
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
}