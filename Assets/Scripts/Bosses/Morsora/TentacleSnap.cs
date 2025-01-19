using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleSnap : MonoBehaviour
{
    public LayerMask playerLayer;
    public float Damage { get; private set; } = 10f;
    public int NumberOfSlams = 1;
    private BoxCollider2D hitBox;
    private int currentSlamCount = 0;
    private Animator animator;

    [SerializeField] private GameObject telegraphPrefab;

    private void Awake()
    {
        // We're just using a collider to "set the hitbox". We could do it differently if we want.
        hitBox = GetComponent<BoxCollider2D>();
        if (hitBox == null)
        {
            Debug.LogError("TentacleSnap: No BoxCollider2D found on this object!");
        }
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("TentacleSnap: No Animator found on this object!");
        }
        AbilityObjectManager.Instance.RegisterAbilityObject(transform.parent.gameObject);
    }

    public void Initialize(float damage, int numberOfSlams)
    {
        Damage = damage;
        NumberOfSlams = numberOfSlams;
        currentSlamCount = 0;
    }

    private void Hit()
    {
        Collider2D[] hitColliders = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(playerLayer);
        Debug.Log("OverlapBox Center: " + ((Vector2)transform.position + hitBox.offset) + ", Size: " + hitBox.size);
        int numColliders = Physics2D.OverlapCollider(hitBox, filter, hitColliders);

        if (numColliders > 0)
        {
            Debug.Log(numColliders + " tentacle snap hit");
            for (int i = 0; i < numColliders; i++)
            {
                PlayerHealth playerHealth = hitColliders[i].GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log("TentacleSnap: Hit player");
                    playerHealth.TakeDamage(Damage);
                    break;
                }
            }
        }
    }

    private void SpawnTelegraph(float fadeDuration = 1.5f)
    {
        float angle = transform.parent.rotation.eulerAngles.z;
        Quaternion rotation = Quaternion.Euler(0, 0, angle + 90);
        GameObject telegraph = Instantiate(telegraphPrefab, transform.parent.position, rotation);
        telegraph.GetComponent<SpearTelegraph>().fadeDuration = fadeDuration;
        telegraph.transform.parent = transform.parent;
        telegraph.transform.localPosition = new Vector3(0, -0.25f, 0); // we love bandaids in this house
        

    }

    public void SlamFinished()
    {
        currentSlamCount++;
        if (currentSlamCount >= NumberOfSlams)
        {
            AbilityObjectManager.Instance.UnregisterAbilityObject(transform.parent.gameObject);
            Destroy(transform.parent.gameObject);
        }
    }
}
