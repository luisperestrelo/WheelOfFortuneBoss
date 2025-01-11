using UnityEngine;
using UnityEngine.UI;

public class ChargeableWheelArea : WheelArea
{
    [Header("Charge Settings")]
    [SerializeField] private float chargeUpSpeed = 1f;
    [SerializeField] private float decaySpeed = 0.5f;
    [SerializeField] private bool resetOnExit = true;
    [SerializeField] private float maxCharge = 100f;

    [Header("Feedback")]
    [SerializeField] private Slider chargeSlider;
    [SerializeField] private float volumeReducSpeed = 1;
    [SerializeField] private AudioClip strikeSfx;

    [Header("Powerful Attack")]
    [SerializeField] private GameObject powerfulAttackPrefab; 
    [SerializeField] private Transform spawnPointOfAttack; // eg this could be mouse position in some cases

    private float currentCharge = 0f;
    private bool isCharging = false;

    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
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
        audioSource.pitch = Mathf.Lerp(0, 3, currentCharge / maxCharge);
        audioSource.volume = 1;
        if (currentCharge >= maxCharge)
        {
            currentCharge = maxCharge;
            TriggerPowerfulAttack();
        }
    }

    private void Decay()
    {
        currentCharge -= decaySpeed * Time.deltaTime;
        audioSource.pitch = Mathf.Lerp(0, 3, currentCharge / maxCharge);
        audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime * volumeReducSpeed);
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
            //audioSource.PlayOneShot(strikeSfx);
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