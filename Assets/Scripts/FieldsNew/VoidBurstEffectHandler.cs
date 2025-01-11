using UnityEngine;

//Not using, using the ChargedVoidBurstEffectHandler instead
public class VoidBurstEffectHandler : FieldEffectHandler
{
    private float damageAmount;
    private BaseAttack voidBurstAttack;
    private float curseDuration;
    private float curseDamageAmount;
    private float curseDamageInterval;
    private float maxTimeInField;
    private float timeInField;
    private bool isCursed;
    private Coroutine curseCoroutine;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        VoidBurstField voidBurstFieldData = (VoidBurstField)fieldData;
        damageAmount = voidBurstFieldData.DamageAmount;
        voidBurstAttack = voidBurstFieldData.VoidBurstAttack;
        curseDuration = voidBurstFieldData.CurseDuration;
        curseDamageAmount = voidBurstFieldData.CurseDamageAmount;
        curseDamageInterval = voidBurstFieldData.CurseDamageInterval;
        maxTimeInField = voidBurstFieldData.MaxTimeInField;
        timeInField = 0f;
        isCursed = false;
    }

    public override void OnEnter(Player player)
    {
        Debug.Log("Entering Void Burst Field");
        timeInField = 0f;
        player.GetComponent<PlayerCombat>().CurrentAttack = voidBurstAttack;
    }

    public override void OnStay(Player player, float deltaTime)
    {
        timeInField += deltaTime;
        if (timeInField >= maxTimeInField)
        {
            if (!isCursed)
            {
                ApplyCurse(player);
            }
            else
            {
                RefreshCurse(player);
            }
            timeInField = 0f; // Reset timeInField after applying or refreshing the curse
        }
    }

    public override void OnExit(Player player)
    {
        Debug.Log("Exiting Void Burst Field");
        player.GetComponent<PlayerCombat>().CurrentAttack = null; // Reset to default attack
    }

    private void ApplyCurse(Player player)
    {
        isCursed = true;
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

    private System.Collections.IEnumerator CurseDamageOverTime(Player player)
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