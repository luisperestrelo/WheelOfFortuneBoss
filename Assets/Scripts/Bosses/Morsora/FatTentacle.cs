using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatTentacle : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    public Wall wall;


    public void Initialize(float damage)
    {
        this.damage = damage;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    private void Start()
    {
        AbilityObjectManager.Instance.RegisterAbilityObject(transform.parent.gameObject);
    }

    
    private void OnDestroy()
    {
        AbilityObjectManager.Instance.UnregisterAbilityObject(transform.parent.gameObject);
    }
}
