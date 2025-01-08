using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage;
    protected Rigidbody2D rb;

    [SerializeField] private AudioClip spawnSfx;
    [SerializeField] private AudioClip hitSfx;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SFXPool.instance.PlaySound(spawnSfx);
    }

    public virtual void SetDamage(float damage)
    {
        _damage = damage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Health>().TakeDamage(_damage);
            SFXPool.instance.PlaySound(hitSfx);
            Destroy(gameObject);
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }
}
