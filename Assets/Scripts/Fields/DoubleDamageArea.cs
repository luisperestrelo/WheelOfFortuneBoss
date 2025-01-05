using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamageArea : WheelEffect
{
    [SerializeField] private float _damageMultiplier = 2f;
    [SerializeField] private float _duration = 5f;

    private bool _alreadyApplied = false;
    public override void OnUpdate(PlayerCombat player)
    {
        base.OnUpdate(player);
        if (!_alreadyApplied)
            player.SetDamageMultiplierForDuration(_damageMultiplier, _duration);
    }

    private IEnumerator CooldownRoutine()
    {
        _alreadyApplied = true;
        yield return new WaitForSeconds(_duration);
        _alreadyApplied = false;
    }
}
