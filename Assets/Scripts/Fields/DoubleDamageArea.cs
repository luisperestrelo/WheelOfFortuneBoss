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
        { //TODO: need to refactor this kind of stuff
            _playerCombat.SetDamageMultiplierForDuration(_damageMultiplier, _duration);
            _playerCombat.SetGlobalDamageMultiplier(_damageMultiplier);
            _alreadyApplied = true;
        }
    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        _alreadyApplied = false;

        // Start a coroutine to reset the damage multiplier after the duration, this is a bandaid quick fix
        // later I want some form of centralized buff system
        StartCoroutine(ResetDamageMultiplierAfterDuration());
    }

    //It's buggy atm because it can start multiple coroutines, but no need to fix now since it will all be refactored
    private IEnumerator ResetDamageMultiplierAfterDuration()
    {
        yield return new WaitForSeconds(_duration);

        _playerCombat.SetGlobalDamageMultiplier(1f);


    }
}
