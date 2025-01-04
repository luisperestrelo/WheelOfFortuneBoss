using System.Collections;
using UnityEngine;

/// <summary>
/// This script is used to play out the boss fight, with the boss abilities. 
/// </summary>
/// TODO: Later we should do this with a state machine
public class BossControllerSimple : MonoBehaviour
{
    public GameObject purpleSlashPrefab;
    public Transform player;

    [SerializeField]
    private float slashCd = 2.5f;

    void Start()
    {
        StartCoroutine(SlashLoopRoutine());
    }

    private IEnumerator SlashLoopRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(slashCd);
            if (player != null)
                FireSlash();
        }
    }

    private void FireSlash()
    {
        GameObject purpleSlash = Instantiate(purpleSlashPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        purpleSlash.transform.rotation = Quaternion.Euler(0, 0, angle);
        purpleSlash.GetComponent<Rigidbody2D>().velocity = direction * 20f;
    }
} 