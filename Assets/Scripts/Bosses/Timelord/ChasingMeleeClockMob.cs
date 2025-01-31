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

    [SerializeField] private SpriteRenderer visuals;
    [SerializeField] private float maxRotationDegrees = 30;

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
            //RotateTowardsPlayer();
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

    public void DebugAnimationEvent()
    {
        Debug.Log("Animation Event!");
    }

    private void FaceThePlayer()
    {
        GameObject healthBar = transform.Find("Healthbar").gameObject;
        
        if (player.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        
        healthBar.transform.rotation = Quaternion.identity;

        healthBar.transform.localPosition = new Vector3(0f, 5f, 0f); 
    }

    private void RotateTowardsPlayer()

    {
        Vector3 direction = player.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        // Some Minions might not have a dedicated visuals game object 
        var transformToRotate = visuals ? visuals.transform : transform;

        if (direction.x > 0)
        {
            angle = Mathf.Clamp(angle, -maxRotationDegrees, maxRotationDegrees);

            var scale = transformToRotate.localScale;
            if (scale.x < 0)
                transformToRotate.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        // flip X 
        else
        {
            angle = Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg;
            angle = -Mathf.Clamp(angle, -30, 30);

            var scale = transformToRotate.localScale;
            if (scale.x > 0)
                transformToRotate.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
        }

        transformToRotate.rotation = Quaternion.Euler(0, 0, angle);
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
