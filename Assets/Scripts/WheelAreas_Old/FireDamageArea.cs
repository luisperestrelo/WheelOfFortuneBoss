using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamageArea : WheelArea
{
    [SerializeField] private float _attackRate = 0.5f;

    [SerializeField]
    private Projectile _fireballPrefab;
    private float _nextAttackTime = 0f;
    


    protected override void Update()
    {
        base.Update();
        if (_isPlayerInArea)
        {
            //Debug.Log("Player is in area name: " + gameObject.name);
            if (Time.time >= _nextAttackTime)
            {
                //_playerCombat.SetProjectileType(_fireballPrefab);
                _nextAttackTime = Time.time + _attackRate;
            }   


        }
        else;
            //Obviously bad for when multiple zones are on the wheel but it works for the prototype
            //_playerCombat.SetProjectileType(null);
    }
    
}
