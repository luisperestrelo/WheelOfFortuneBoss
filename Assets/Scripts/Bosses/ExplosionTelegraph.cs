using UnityEngine;
using System.Collections;

public class ExplosionTelegraph : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float telegraphDuration = 1f;



    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        Destroy(gameObject, telegraphDuration);
    }

    public float GetTelegraphDuration()
    {
        return telegraphDuration;
    }   

    public void SetTelegraphDuration(float duration)
    {
        telegraphDuration = duration;
    }


    ///Unlike the Spear, this one doesnt fade out

/*     private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        Destroy(gameObject);
    } */
} 