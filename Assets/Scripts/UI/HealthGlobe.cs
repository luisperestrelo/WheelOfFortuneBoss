using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthGlobe : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Image whiteFillImage;
    private Health targetHealth;

    private Coroutine reduceFillCoroutine;
    private Coroutine reduceWhiteCoroutine;

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
        // if (reduceFillCoroutine != null)
        // {
        //     StopCoroutine(reduceFillCoroutine);
        // }
        // reduceFillCoroutine = StartCoroutine(ReduceFill(fillAmount));
        //
        
        if (reduceWhiteCoroutine != null)
        {
            StopCoroutine(reduceWhiteCoroutine);
        }
        reduceWhiteCoroutine = StartCoroutine(ReduceWhite(fillAmount));
    }

    private IEnumerator ReduceFill(float toFillAmount)
    {
        var duration = 2f;
        var elapsedTime = 0f;
        
        var fromFill = fillImage.fillAmount;
        var fromWhite = whiteFillImage.fillAmount;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
             
            fillImage.fillAmount = Mathf.Lerp(fromFill, toFillAmount, elapsedTime / duration);
            yield return null;
        }
    }
    
    private IEnumerator ReduceWhite(float toFillAmount)
    {
        var duration = 1f;
        var elapsedTime = 0f;
        
        var fromWhite = whiteFillImage.fillAmount;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
             
            whiteFillImage.fillAmount = Mathf.Lerp(fromWhite, toFillAmount, elapsedTime / duration);
            yield return null;
        }
    }
} 