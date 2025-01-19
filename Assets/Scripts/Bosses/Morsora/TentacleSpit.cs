using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleSpitter : MonoBehaviour
{

    public float Damage { get; private set; } = 10f;
    public int NumberOfSpits = 1;
    public GameObject tentacleSpitPrefab;
    private int currentSpitCount = 0;
    public float projectileSpeed = 10f;
    public float shootingCooldown = 1f;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        AbilityObjectManager.Instance.RegisterAbilityObject(transform.gameObject);
    }


    public void Initialize(float damage, int numberOfSpits, float projectileSpeed, float shootingCooldown)
    {
        Damage = damage;
        NumberOfSpits = numberOfSpits;
        this.projectileSpeed = projectileSpeed;
        this.shootingCooldown = shootingCooldown;
        currentSpitCount = 0;
    }
    public void Attack()
    {
        if (currentSpitCount < NumberOfSpits)
        {
            
            GameObject tentacleSpit = Instantiate(tentacleSpitPrefab, transform.position, transform.rotation);
            
            if (tentacleSpit != null)
            {
                EnemyProjectile proj = tentacleSpit.GetComponent<EnemyProjectile>();
                if (proj != null)
                {
                    proj.SetDamage(Damage);
                    var velocity = -tentacleSpit.transform.right * projectileSpeed;
                    var v2 = velocity.normalized;
                    float angle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg ;
                    proj.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
                    proj.SetVelocity(velocity);
                }
            }
            currentSpitCount++;

            if (currentSpitCount >= NumberOfSpits)
            {
                animator.SetBool("DoneShooting", true);
            }
        }

        else // shouldnt get here
        {
            animator.SetBool("DoneShooting", true);
        }

    }

    public void SelfDestroy()
    {
        AbilityObjectManager.Instance.UnregisterAbilityObject(transform.gameObject);
        Destroy(gameObject);
    }
}
