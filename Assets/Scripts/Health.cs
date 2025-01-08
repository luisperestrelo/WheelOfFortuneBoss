using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth; // tbh, should be private but I like being able to see it in the inspector without going Debug mode
    
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
            return;
        }

        currentHealth -= damageAmount;
        OnHealthChanged.Invoke(currentHealth, maxHealth);
        Debug.Log(gameObject.name + " took " + damageAmount + " damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}