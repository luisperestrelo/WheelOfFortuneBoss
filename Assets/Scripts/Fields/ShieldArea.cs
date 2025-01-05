using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldArea : WheelEffect
{
    [Header("Shield Area Settings")]
    [SerializeField] private GameObject shieldPrefab;         
    [SerializeField] private GameObject onCooldownTextObject; 
    [SerializeField] private float cooldownDuration = 5f;     

    private bool _isOnCooldown = false;
    private float _cooldownTimer = 0f;

    public override void OnUpdate(PlayerCombat player)
    {
        base.OnUpdate(player);

        if (!_isOnCooldown && !player.HasShield)
        {
            player.ActivateShield(shieldPrefab, this);
            player.StartCoroutine(CooldownRoutine());
            player.StartCoroutine(ShieldTimerRoutine(player));
        }

    }

    private IEnumerator CooldownRoutine()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        _isOnCooldown = false;
    }

    private IEnumerator ShieldTimerRoutine(PlayerCombat player)
    {
        yield return new WaitForSeconds(0.25f);
        player.RemoveShield();
    }
}
