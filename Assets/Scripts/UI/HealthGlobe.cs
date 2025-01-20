using UnityEngine;
using UnityEngine.UI;

public class HealthGlobe : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private Health targetHealth;

    private void Start()
    {
        // Get the Health component from the player object
        targetHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>(); // Assuming the player has the "Player" tag
        if (targetHealth == null)
        {
            Debug.LogError("HealthGlobe could not find the Player's Health component.");
            return;
        }

        // Subscribe to events
        targetHealth.OnHealthChanged.AddListener(OnHealthChanged);
    }

    public void OnHealthChanged(float currentHealth, float maxHealth)
    {
        float fillAmount = currentHealth / maxHealth;
        fillImage.fillAmount = fillAmount;
    }
} 