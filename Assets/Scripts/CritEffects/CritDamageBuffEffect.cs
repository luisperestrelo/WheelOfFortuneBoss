using UnityEngine;

public class CritDamageBuffEffect : IOnCritEffect
{
    private float _duration = 5f;
    private float _damageMultiplier = 1.1f;
    public CritDamageBuffEffect(float damageMultiplier, float duration) 
    {
        _duration = duration;
        _damageMultiplier = damageMultiplier;
    }

    public void HandleCrit(PlayerCombat playerCombat, BuffManager buffManager, Stats stats)
    {
        var buff = new CritDamageBuff(_damageMultiplier, _duration);
        buffManager.ApplyBuff(buff);
    }
} 