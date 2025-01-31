using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class ActivateRendRealitySlice : MonoBehaviour
{
    [Header("Alpha Settings")]
    [SerializeField] private int inlineInitialAlpha = 0;
    [SerializeField] private int inlineFinalAlpha = 255;
    [SerializeField] private int outlineInitialAlpha = 0;
    [SerializeField] private int outlineFinalAlpha = 255;

    [Header("Light Settings")]
    [SerializeField] private float lightInitialIntensity = 0f;
    [SerializeField] private float lightFinalIntensity = 10f;

    [Header("Activation Timing")]
    [Tooltip("How many seconds it takes to fully activate the rend slice.")]
    [SerializeField] private float activationTime = 2f;

    [Header("Activation Style")]
    [Tooltip("If true, alpha and light intensity will linearly interpolate. If false, values instantly switch at the end.")]
    [SerializeField] private bool isLinearActivation = true;

    [Header("Damage Settings")]
    [Tooltip("Damage per second that this slice inflicts once active.")]
    [SerializeField] private float damagePerSecond = 10f;
    [Tooltip("Interval at which damage is applied, in seconds.")]
    [SerializeField] private float damageInterval = 0.4f;

    [SerializeField] private bool isFastRend = false;

    private float damageTimer = 0f;
    private bool isPlayerInZone = false;

    private SpriteRenderer inlineRenderer;
    private SpriteRenderer outlineRenderer;

    private Light2D telegraphLight;

    private Collider2D damageCollider;

    // Cache the PlayerHealth for dealing damage
    private PlayerHealth playerHealth;

    private void Awake()
    {
        Transform rendTelegraph;
        if (isFastRend)
        {
            rendTelegraph = transform.Find("Rend Telegraph");
        }
        else
        {
            rendTelegraph = transform.Find("Rend Telegraph 1");
        }
        
        if (rendTelegraph == null)

        {
            Debug.LogWarning("Could not find child object 'Rend Telegraph'.");
            return;
        }

        Transform inlineChild = rendTelegraph.Find("Rend-Telegraph-inline");
        if (inlineChild != null)
            inlineRenderer = inlineChild.GetComponent<SpriteRenderer>();

        Transform outlineChild = rendTelegraph.Find("Rend-Telegraph-otline");
        if (outlineChild != null)
            outlineRenderer = outlineChild.GetComponent<SpriteRenderer>();

        Transform lightChild = rendTelegraph.Find("Light 2D");
        if (lightChild != null)
            telegraphLight = lightChild.GetComponent<Light2D>();

        damageCollider = GetComponent<Collider2D>();
        if (damageCollider == null)
        {
            Debug.LogWarning("Could not find damage collider on this object. Please assign one.");
        }
    }

    private void Start()
    {

        playerHealth = FindObjectOfType<PlayerHealth>();

        // Disable the collider before activation completes
        if (damageCollider != null)
            damageCollider.enabled = false;

        // If we are NOT linearly activating, we should set all visuals to initial at the start:
        if (!isLinearActivation)
        {
            SetVisualsToInitial();
        }

        StartCoroutine(ActivationRoutine());
    }

    private void Update()
    {
        // If the collider is active and the player is in the zone, handle damage
        if (damageCollider != null && damageCollider.enabled && isPlayerInZone)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damagePerSecond * damageInterval);
                }
                damageTimer = 0f;
            }
        }
    }

    private IEnumerator ActivationRoutine()
    {
        // If we are linearly activating, we gradually interpolate over activationTime
        if (isLinearActivation)
        {
            float elapsedTime = 0f;
            while (elapsedTime < activationTime)
            {
                float t = elapsedTime / activationTime;

                float inlineA = Mathf.Lerp(inlineInitialAlpha / 255f, inlineFinalAlpha / 255f, t);
                float outlineA = Mathf.Lerp(outlineInitialAlpha / 255f, outlineFinalAlpha / 255f, t);
                float currentIntensity = Mathf.Lerp(lightInitialIntensity, lightFinalIntensity, t);

                SetVisuals(inlineA, outlineA, currentIntensity);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure final values
            SetVisuals(inlineFinalAlpha / 255f, outlineFinalAlpha / 255f, lightFinalIntensity);
        }
        else
        {
            // If not linearly activating, wait for the full duration
            yield return new WaitForSeconds(activationTime);

            // Then instantly set to final
            SetVisualsToFinal();
        }

        // Enable the collider to start dealing damage
        if (damageCollider != null)
            damageCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the player
        if (other.CompareTag("Player"))
        {
            // If we are already active, apply immediate "entry" damage
            if (damageCollider != null && damageCollider.enabled)
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damagePerSecond * damageInterval);
                }
            }
            isPlayerInZone = true;
            damageTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If the player leaves the zone, stop dealing damage
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            damageTimer = 0f;
        }
    }

    /// <summary>
    /// Helper method to continuously set visuals (for the linear fade).
    /// </summary>
    private void SetVisuals(float inlineA, float outlineA, float lightIntensity)
    {
        if (inlineRenderer != null)
        {
            Color c = inlineRenderer.color;
            c.a = inlineA;
            inlineRenderer.color = c;
        }

        if (outlineRenderer != null)
        {
            Color c = outlineRenderer.color;
            c.a = outlineA;
            outlineRenderer.color = c;
        }

        if (telegraphLight != null)
        {
            telegraphLight.intensity = lightIntensity;
        }
    }

    /// <summary>
    /// Set all visuals to their "initial" alpha/light values.
    /// </summary>
    private void SetVisualsToInitial()
    {
        SetVisuals(
            inlineInitialAlpha / 255f,
            outlineInitialAlpha / 255f,
            lightInitialIntensity
        );
    }

    /// <summary>
    /// Set all visuals to their "final" alpha/light values.
    /// </summary>
    private void SetVisualsToFinal()
    {
        SetVisuals(
            inlineFinalAlpha / 255f,
            outlineFinalAlpha / 255f,
            lightFinalIntensity
        );
    }
}
