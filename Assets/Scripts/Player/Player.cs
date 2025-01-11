using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private WheelManager wheelManager;
    [SerializeField] private PlayerSpinMovement playerSpinMovement;

    private WheelSegment currentSegment;

    private void Start()
    {
        //Get the WheelManager component from the same GameObject this is attached to,
        //or find it in the scene if it's located elsewhere.
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
            }

            // Enter the new segment
            currentSegment = newSegment;

            if (currentSegment != null)
            {
                currentSegment.EffectHandler.OnEnter(this);
            }
        }

        // Update the current segment (for OnStay effects)
        if (currentSegment != null)
        {
            currentSegment.EffectHandler.OnStay(this, Time.deltaTime);
        }
    }
} 