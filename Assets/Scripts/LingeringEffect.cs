using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Effects that linger for a duration after the player leaves a field
//TODO: not actually using this yet, but the idea would be that all lingering buffs inherit from this class
public class LingeringEffect : MonoBehaviour

{
    [SerializeField] protected float _lingerDuration = 5f;
    protected float _lingerTimer = 0f;

    protected virtual void Update()
    {
        _lingerTimer += Time.deltaTime;
        if (_lingerTimer >= _lingerDuration)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void ApplyEffect()
    {
        //Apply effect to player
    }
}
