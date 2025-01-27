using UnityEngine;

public class CritChanceBuffEffectHandler : FieldEffectHandler
{
    private CritChanceBuffField critFieldData;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        critFieldData = fieldData as CritChanceBuffField;
        if (critFieldData == null)
        {
            Debug.LogError("[CritChanceBuffEffectHandler] Provided FieldData is not CritChanceBuffField.");
        }
    }

    public override void OnEnter(Player player)
    {
        if (player == null) return;
        var buffManager = player.GetComponent<BuffManager>();
        if (buffManager != null)
        {
            float finalCritChance = critFieldData.CritChanceIncrease * playerStats.LingeringBuffFieldsEffectivenessMultiplier;
            float finalDuration = critFieldData.BuffDuration * playerStats.LingeringBuffFieldsDurationMultiplier;
            var buff = new MinorCritBuff(
                finalCritChance,
                finalDuration
            );
            buffManager.ApplyBuff(buff);
        }
    }

    public override void OnStay(Player player, float deltaTime)
    {
        if (player == null) return;
        var buffManager = player.GetComponent<BuffManager>();
        if (buffManager != null)
        {
            float finalCritChance = critFieldData.CritChanceIncrease * playerStats.LingeringBuffFieldsEffectivenessMultiplier;
            float finalDuration = critFieldData.BuffDuration * playerStats.LingeringBuffFieldsDurationMultiplier;
            var buff = new MinorCritBuff(
                finalCritChance,
                finalDuration
            );
            buffManager.ApplyBuff(buff);
        }
    }

    public override void OnExit(Player player)
    {
        // Not needed here (the duration is handled by BuffManager).
    }
}