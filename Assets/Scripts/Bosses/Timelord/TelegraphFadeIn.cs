using UnityEngine;

public class TelegraphFadeIn : MonoBehaviour
{
    public float fadeInDuration = 1.0f;
    public float innerTargetAlpha = 0.255f; // Alpha value for the "Inner" sprite (65/255)

    private SpriteRenderer borderRenderer;
    private SpriteRenderer innerRenderer;
    private float timer = 0f;

    void Start()
    {
        Transform borderTransform = transform.Find("Border");
        Transform innerTransform = borderTransform.transform.Find("Inner");

        if (borderTransform == null || innerTransform == null)
        {
            Debug.LogError("Border or Inner child not found. Ensure your prefab has 'Border' and 'Inner' children.");
            enabled = false;
            return;
        }

        borderRenderer = borderTransform.GetComponent<SpriteRenderer>();
        innerRenderer = innerTransform.GetComponent<SpriteRenderer>();

        if (borderRenderer == null || innerRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on Border or Inner child.");
            enabled = false;
            return;
        }

        Color borderStartColor = borderRenderer.color;
        borderStartColor.a = 0f;
        borderRenderer.color = borderStartColor;

        Color innerStartColor = innerRenderer.color;
        innerStartColor.a = 0f;
        innerRenderer.color = innerStartColor;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < fadeInDuration)
        {
            float normalizedTime = timer / fadeInDuration;
            Color borderColor = borderRenderer.color;
            borderColor.a = Mathf.Lerp(0f, 1f, normalizedTime);
            borderRenderer.color = borderColor;

            Color innerColor = innerRenderer.color;
            innerColor.a = Mathf.Lerp(0f, innerTargetAlpha, normalizedTime);
            innerRenderer.color = innerColor;
        }
        else
        {
            Color borderColor = borderRenderer.color;
            borderColor.a = 1f;
            borderRenderer.color = borderColor;

            Color innerColor = innerRenderer.color;
            innerColor.a = innerTargetAlpha;
            innerRenderer.color = innerColor;
        }
    }
}