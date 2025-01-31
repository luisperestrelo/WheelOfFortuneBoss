using UnityEngine;

public class WagerOfAeonsProjectile : BaseProjectile
{
    private void Start()
    {
        transform.up = velocity;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); 
    }
} 