using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Health targetHealth;
    private void Start()
    {

        targetHealth = FindObjectOfType<PlayerHealth>();

        targetHealth.OnHealthChanged.AddListener(OnHealthChanged);
        targetHealth.OnDie.AddListener(OnDie);

        OnHealthChanged(targetHealth.GetCurrentHealth(), targetHealth.GetMaxHealth());
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        if (healthText != null)
        {
            //no decimal places
            healthText.text = $"{Mathf.Round(currentHealth)} / {Mathf.Round(maxHealth)}";
        }
    }

    private void OnDie()
    {
        if (healthText != null)
        {
            healthText.text = "0";
        }
    }
}
