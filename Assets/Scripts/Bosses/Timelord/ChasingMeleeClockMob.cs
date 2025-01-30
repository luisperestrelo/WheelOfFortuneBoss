using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingMeleeClockMob : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float meleeRange = 1.5f; // Distance to trigger melee attack
    [SerializeField] private Animator animator; 
    private Transform player;
    private PlayerHealth playerHealth;
    private bool isAttacking = false; 
    [SerializeField] private Transform frontForMeleeHit; 
    [SerializeField] private float meleeHitRadius = 0.75f; // Radius of the melee hit "swing"
    [SerializeField] private LayerMask playerLayer; 

    //ideally we would use a statemachine, but this is a quick and dirty solution
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        Initialize(player, speed, damage);
    }

    public void Initialize(Transform player, float speed, float damage)
    {
        this.player = player;
        this.playerHealth = player.GetComponent<PlayerHealth>();
        this.speed = speed;
        this.damage = damage;
    }

    void Update()
    {
        if (player == null || isAttacking) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= meleeRange)
        {
            StartMeleeAttack();
        }
        else
        {
            // Chase the player if not in melee range, otherwise attack again
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * (speed * Time.deltaTime);

            FaceThePlayer();
        }
    }

    private void StartMeleeAttack()
    {
        isAttacking = true; 
        animator.SetTrigger("Melee"); 
        // Damage will be applied through animation event
    }

    //this is called by an animation event
    public void MeleeAttackHit()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(frontForMeleeHit.position, meleeHitRadius, playerLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player")) 
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    Debug.Log("Melee Hit Player!");
                    break; 
                }
            }
        }
        isAttacking = false; // Allow movement and attacks again after attack animation
    }

    private void FaceThePlayer()
    {
        if (player.position.x > transform.position.x)
        {

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f); 
        }
    }

    private void OnDrawGizmosSelected() 
    {
        if (frontForMeleeHit != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(frontForMeleeHit.position, meleeHitRadius);
        }
    }
}
