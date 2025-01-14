using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public abstract class ChargeableFieldEffectHandler : FieldEffectHandler
{
    protected float chargeTime;
    protected float currentChargeTime;
    protected bool isCharging;
    protected bool isDecaying;
    protected bool resetsOnExit;
    protected float decayRate;
    protected Image chargeIndicatorImage;

    protected AudioSource chargeSource;

    public float ChargePercent => currentChargeTime / chargeTime;


    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        // Cast to ChargeableField to access chargeable-specific properties
        ChargeableField chargeableFieldData = (ChargeableField)fieldData;
        chargeTime = chargeableFieldData.ChargeTime;
        resetsOnExit = chargeableFieldData.ResetsOnExit;
        decayRate = chargeableFieldData.DecayRate;
        currentChargeTime = 0f;
        isCharging = false;
        isDecaying = false;

        if (chargeSource == null)
        {
            chargeSource = CreateAudioSource(true);
            chargeSource.clip = Resources.Load("SE_Field_Charge") as AudioClip;
            chargeSource.loop = true;
            chargeSource.volume = 0;
            chargeSource.Play();
        }


    }

    public override void OnEnter(Player player)
    {
        isCharging = true;
        isDecaying = false;
    }

    public override void OnStay(Player player, float deltaTime)
    {
        if (isCharging)
        {
            // Apply the charge-up field speed multiplier
            currentChargeTime += deltaTime * playerStats.ChargeUpFieldsSpeedMultiplier;
            if (currentChargeTime >= chargeTime)
            {
                OnChargeComplete(player);
                currentChargeTime = 0f;
            }
        }
    }

    public override void OnExit(Player player)
    {
        isCharging = false;
        HandleChargeDecay();
    }

    protected virtual void HandleChargeDecay()
    {
        if (resetsOnExit)
        {
            currentChargeTime = 0f;
        }
        else
        {
            isDecaying = true;
        }
    }

    protected virtual void Update()
    {
        UpdateChargeSource();
        if (isDecaying)
        {
            // Apply the decaying charge-up field decay slowdown multiplier
            currentChargeTime -= decayRate * Time.deltaTime * playerStats.DecayingChargeUpFieldsDecaySlowdownMultiplier;
            currentChargeTime = Mathf.Clamp(currentChargeTime, 0f, chargeTime);

            if (currentChargeTime == 0)
            {
                isDecaying = false;
            }
        }
        if (chargeIndicatorImage != null)
        {
            chargeIndicatorImage.fillAmount = currentChargeTime / chargeTime;
        }
    }

    protected void UpdateChargeSource()
    {
        chargeSource.pitch = 1 + currentChargeTime / chargeTime;
        if (isCharging)
            chargeSource.volume = currentChargeTime / chargeTime;
        else
            chargeSource.volume = Mathf.MoveTowards(chargeSource.volume, 0, Time.deltaTime);
    }

    //UI stuff
    public virtual void OnChargeUpdate(Player player, float chargePercent) { }

    protected abstract void OnChargeComplete(Player player);

    public virtual void SetChargeIndicatorImage(Image image)
    {
        chargeIndicatorImage = image;
        if (chargeIndicatorImage != null)
        {
            chargeIndicatorImage.fillAmount = 0f; // Initialize to 0
        }
    }
}