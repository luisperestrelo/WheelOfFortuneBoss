using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public UnityEvent<float, float> OnHealthChanged;

    public UnityEvent OnDie;

    protected virtual void Start()
    {
        currentHealth = maxHealth;



        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }

    public virtual void TakeDamage(float damageAmount)
    {
        PlayerCombat pc = GetComponent<PlayerCombat>();
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