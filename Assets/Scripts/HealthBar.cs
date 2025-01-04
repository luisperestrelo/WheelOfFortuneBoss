using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private Transform target;
    private Health targetHealth;

    private void Start()
    {
        targetHealth = GetComponentInParent<Health>();
        if (targetHealth == null)
        {
            Debug.LogError("HealthBar must be a child of an object with a Health component.");
            return;
        }

        targetHealth.OnHealthChanged.AddListener(OnHealthChanged);
        targetHealth.OnDie.AddListener(OnDie);

        target = targetHealth.transform;
    }

    private void Update()
    {
        FollowTarget();
    }

    public void OnHealthChanged(float currentHealth, float maxHealth)
    {
        float fillAmount = currentHealth / maxHealth;
        fillImage.fillAmount = fillAmount;
        Show(); 
    }

    private void OnDie()
    {

        Destroy(gameObject);
    }

    private void FollowTarget()
    {
        if (target != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
} 