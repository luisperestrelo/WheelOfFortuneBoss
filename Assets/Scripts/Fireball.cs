using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float _damage;

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Health>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
