
using UnityEngine;

public class CritAttackSpeedBuffEffect : IOnCritEffect
{
    private float _duration = 5f;
    private float _attackSpeedMultiplier = 1.4f;

    public CritAttackSpeedBuffEffect(float attackSpeedMultiplier, float duration)
    {
        _duration = duration;
        _attackSpeedMultiplier = attackSpeedMultiplier;
    }

    public void HandleCrit(PlayerCombat playerCombat, BuffManager buffManager, Stats stats)
    {
        var buff = new CritAttackSpeedBuff(_attackSpeedMultiplier, _duration);
        buffManager.ApplyBuff(buff);
    }


}   