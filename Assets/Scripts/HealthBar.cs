using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Image whiteFillImage;
    private Transform target;
    private Health targetHealth;

    private Coroutine healthChangeCoroutine;

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

        if (whiteFillImage)
        {
            if(healthChangeCoroutine != null)
                StopCoroutine(healthChangeCoroutine);
            StartCoroutine(ReduceWhiteFill(fillAmount));
        }
  

        fillImage.fillAmount = fillAmount;
        Show(); 
    }

    private IEnumerator ReduceWhiteFill(float targetFillAmount)
    {
        var elapsedTime = 0f;
        var from = whiteFillImage.fillAmount;
        var duration = 1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            whiteFillImage.fillAmount = Mathf.Lerp(from, targetFillAmount, elapsedTime / duration);
            
            yield return null;
        }
        
    }

    private void OnDie()
    {
        if(healthChangeCoroutine != null)
            StopCoroutine(healthChangeCoroutine);
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