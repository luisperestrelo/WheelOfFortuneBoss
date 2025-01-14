using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private WheelManager wheelManager;
    [SerializeField] private PlayerSpinMovement playerSpinMovement;

    //Wheel SFX are handled by the player because the field effect SFX are a colossal pain to implement
    [Header("SFX")]
    [SerializeField] private AudioSource wheelStateChangeSource;
    [SerializeField] private AudioClip enterFieldSfx;
    [SerializeField] private AudioClip exitFieldSfx;

    private WheelSegment currentSegment;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
        }
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