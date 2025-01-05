using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Areas/Shield Area")]
public class ShieldArea : WheelEffect
{
    [Header("Shield Area Settings")]
    [SerializeField] private GameObject shieldPrefab;         
    [SerializeField] private GameObject onCooldownTextObject; 
    [SerializeField] private float cooldownDuration = 5f;     

    private bool _isOnCooldown = false;

    public override void OnUpdate(PlayerCombat player)
    {
        base.OnUpdate(player);
        Debug.Log(_isOnCooldown);
        if (!_isOnCooldown)
        {
            Debug.Log("Shielding");
            player.ActivateShield(shieldPrefab, this);
            player.StartCoroutine(CooldownRoutine());
        }

    }

    private IEnumerator CooldownRoutine()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        _isOnCooldown = false;
    }
}
