using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamageArea : WheelArea
{
    [SerializeField] private float _damageMultiplier = 2f;
    [SerializeField] private float _duration = 5f;

    private bool _alreadyApplied = false;
    protected override void Update()
    {
        base.Update();
        if (_isPlayerInArea)
        {
            Debug.Log("Player is in area name: " + gameObject.name);
        }
    }


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (!_alreadyApplied)
        {
            _playerCombat.SetDamageMultiplierForDuration(_damageMultiplier, _duration);
            _alreadyApplied = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        _alreadyApplied = false;
    }
}
