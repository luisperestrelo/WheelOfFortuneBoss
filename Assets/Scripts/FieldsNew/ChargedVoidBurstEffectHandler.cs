using UnityEngine;
using System.Collections;

public class ChargedVoidBurstEffectHandler : ChargeableFieldEffectHandler
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