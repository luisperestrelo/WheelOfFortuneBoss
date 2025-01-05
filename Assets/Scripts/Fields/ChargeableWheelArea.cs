using UnityEngine;
using UnityEngine.UI;

public class ChargeableWheelArea : WheelArea
{
    [Header("Charge Settings")]
    [SerializeField] private float chargeUpSpeed = 1f;
    [SerializeField] private float decaySpeed = 0.5f;
    [SerializeField] private bool resetOnExit = true;
    [SerializeField] private float maxCharge = 100f;

    [Header("Visual Feedback")]
    [SerializeField] private Slider chargeSlider;

    [Header("Powerful Attack")]
    [SerializeField] private GameObject powerfulAttackPrefab; 
    [SerializeField] private Transform spawnPointOfAttack; // eg this could be mouse position in some cases

    private float currentCharge = 0f;
    private bool isCharging = false;

    protected override void Start()
    {
        base.Start();
        if (chargeSlider != null)
        {
            chargeSlider.maxValue = maxCharge;
            chargeSlider.value = currentCharge;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (isCharging)
        {
            ChargeUp();
        }
        else if (!resetOnExit)
        {
            Decay();
        }

        if (chargeSlider != null)
        {
            chargeSlider.value = currentCharge;
        }
    }

    private void ChargeUp()
    {
        currentCharge += chargeUpSpeed * Time.deltaTime;
        if (currentCharge >= maxCharge)
        {
            currentCharge = maxCharge;
            TriggerPowerfulAttack();
        }
    }

    private void Decay()
    {
        currentCharge -= decaySpeed * Time.deltaTime;
        if (currentCharge < 0f)
        {
            currentCharge = 0f;
        }
    }

    private void TriggerPowerfulAttack()
    {
        //bandaid fix with position so the visual feedback is correct
        if (powerfulAttackPrefab != null)
        {
            Instantiate(powerfulAttackPrefab, spawnPointOfAttack.position + new Vector3(0, 4.5f, 0), Quaternion.identity);
        }

        currentCharge = 0f;
        if (chargeSlider != null)
        {
            chargeSlider.value = currentCharge;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Player"))
        {
            isCharging = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Player"))
        {
            isCharging = false;
            if (resetOnExit)
            {
                currentCharge = 0f;
            }
        }
    }
} 