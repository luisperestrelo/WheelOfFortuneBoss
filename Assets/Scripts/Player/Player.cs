using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private WheelManager wheelManager;
    [SerializeField] private PlayerSpinMovement playerSpinMovement;
    [SerializeField] private GameObject vfx;
    private PlayerCombat combatScript;

    [SerializeField] private ParticleSystem deathParticlePrefab;
    [SerializeField] private CanvasGroup deathBackground;

    //Wheel SFX are handled by the player because the field effect SFX are a colossal pain to implement
    [Header("SFX")]
    [SerializeField] private AudioSource wheelStateChangeSource;
    [SerializeField] private AudioClip enterFieldSfx;
    [SerializeField] private AudioClip exitFieldSfx;

    private WheelSegment currentSegment;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        spriteRenderer = GetComponent<SpriteRenderer>();
        combatScript = GetComponent<PlayerCombat>();
    }
    private void Start()
    {

        if (wheelManager == null)
        {
            wheelManager = FindObjectOfType<WheelManager>();
        }
        if (playerSpinMovement == null)
        {
            playerSpinMovement = GetComponent<PlayerSpinMovement>();
            playerSpinMovement.enabled = false;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnAnimation());
    }

    private IEnumerator SpawnAnimation()
    {
        yield return null;
        combatScript.enabled = false;
        spriteRenderer.enabled = false;
        playerSpinMovement.enabled = false;
        vfx.SetActive(false);
        yield return new WaitForSeconds(3.8f);
        spriteRenderer.enabled = true;
        playerSpinMovement.enabled = true;
        combatScript.enabled = true;
        vfx.SetActive(true);
    }

    public void Die()
    {
        combatScript.enabled = false;
        spriteRenderer.enabled = false;
        playerSpinMovement.enabled = false;
        vfx.SetActive(false);
        MusicPlayer.instance.StartSection(MusicPlayer.MusicSection.ambience);
        Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        // Get the current segment from the WheelManager
        WheelSegment newSegment = wheelManager.GetCurrentSegment(playerSpinMovement.CurrentAngle);

        // If the player has entered a new segment
        if (newSegment != currentSegment)
        {
            // Exit the previous segment
            if (currentSegment != null)
            {
                currentSegment.EffectHandler.OnExit(this);
                wheelStateChangeSource.PlayOneShot(exitFieldSfx);
            }

            // Enter the new segment
            currentSegment = newSegment;

            if (currentSegment != null)
            {
                currentSegment.EffectHandler.OnEnter(this);
                wheelStateChangeSource.PlayOneShot(enterFieldSfx);
            }
        }

        // Update the current segment (for OnStay effects)
        if (currentSegment != null)
        {
            currentSegment.EffectHandler.OnStay(this, Time.deltaTime);
        }
    }
} 