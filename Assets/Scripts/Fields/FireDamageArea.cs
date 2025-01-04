using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamageArea : WheelArea
{
    [SerializeField] private float _attackRate = 0.5f;
    
    private float _nextAttackTime = 0f;
    


    protected override void Update()
    {
        base.Update();
        if (_isPlayerInArea)
        {
            Debug.Log("Player is in area name: " + gameObject.name);
            if (Time.time >= _nextAttackTime)
            {
                _playerCombat.ShootFireBall();
                _nextAttackTime = Time.time + _attackRate;
            }   


        }
    }
    
}
