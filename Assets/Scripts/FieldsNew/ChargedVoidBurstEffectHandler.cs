using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChargedVoidBurstEffectHandler : ChargeableFieldEffectHandler // This is different maybe dont inherit idk
{
    private BaseAttack voidBurstAttack;
    private float curseDuration;
    private float curseDamageAmount;
    private float curseDamageInterval;
    private bool isCursed;
    private Coroutine curseCoroutine;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        ChargedVoidBurstField chargedVoidBurstFieldData = (ChargedVoidBurstField)fieldData;
        voidBurstAttack = chargedVoidBurstFieldData.VoidBurstAttack;
        curseDuration = chargedVoidBurstFieldData.CurseDuration;
        curseDamageAmount = chargedVoidBurstFieldData.CurseDamageAmount;
        curseDamageInterval = chargedVoidBurstFieldData.CurseDamageInterval;
        isCursed = false;



    }

    public override void OnStay(Player player, float deltaTime)
    {
        if (isCharging)
        {
            // Apply the charge-up field speed multiplier
            currentChargeTime += deltaTime; //we override so it doesnt get the player stats for charge multi
            if (currentChargeTime >= chargeTime)
            {
                OnChargeComplete(player);
                currentChargeTime = 0f;
            }
        }
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);

        Debug.Log("Entering Charged Void Burst Field");
        player.GetComponent<PlayerCombat>().CurrentAttack = voidBurstAttack;
    }

    public override void OnExit(Player player)
    {
        base.OnExit(player);
        Debug.Log("Exiting Charged Void Burst Field");
        player.GetComponent<PlayerCombat>().CurrentAttack = null; // Reset to default attack
    }

    protected override void Update()
    {
        if (isDecaying)
        {
            // Apply the decaying charge-up field decay slowdown multiplier
            currentChargeTime -= decayRate * Time.deltaTime; // we override so it doesnt get the player stats for decay multi
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

    protected override void OnChargeComplete(Player player)
    {
        if (!isCursed)
        {
            ApplyCurse(player);
        }
        else
        {
            RefreshCurse(player);
        }
    }

    public override void SetChargeIndicatorImage(Image image)
    {
        base.SetChargeIndicatorImage(image);
        if (chargeIndicatorImage != null)
        {
            chargeIndicatorImage.color = Color.red; // since charging up is bad, we want to make it red
        }
    }

    private void ApplyCurse(Player player)
    {
        isCursed = true;
        //TODO visuals
        curseCoroutine = StartCoroutine(CurseDamageOverTime(player));
    }

    private void RefreshCurse(Player player)
    {
        // If there's an active curse, stop it
        if (curseCoroutine != null)
        {
            StopCoroutine(curseCoroutine);
        }

        // Restart the curse coroutine
        curseCoroutine = StartCoroutine(CurseDamageOverTime(player));
    }

    private IEnumerator CurseDamageOverTime(Player player)
    {
        float timer = 0f;
        while (timer < curseDuration)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(curseDamageAmount);
            timer += curseDamageInterval;
            yield return new WaitForSeconds(curseDamageInterval);
        }
        isCursed = false;
        curseCoroutine = null;
    }
}