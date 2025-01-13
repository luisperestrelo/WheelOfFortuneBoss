using UnityEngine;
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
            currentChargeTime += deltaTime;
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




        //Debug.Log("current charge of " + name + " is " + currentChargeTime);
        if (isDecaying)
        {
            currentChargeTime -= decayRate * Time.deltaTime;
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