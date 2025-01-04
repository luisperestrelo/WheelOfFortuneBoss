using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAbilityTester : MonoBehaviour
{
    public GameObject[] fields;
    public GameObject purpleSlashPrefab;
    public Transform player;

    [SerializeField]
    private float slashCd = 2.5f;

    void Start()
    {
        StartCoroutine(SlashLoopRoutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeRandomFieldToFire();
        }
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

    //
    public void ChangeRandomFieldToFire()
    {
        if (fields.Length == 0)
            return;

        int randomIndex = Random.Range(0, fields.Length);
        WheelArea area = fields[randomIndex].GetComponent<WheelArea>();

        area.SetOnFire();
        
        // Start a Coroutine to stop the fire after 3 seconds
        StartCoroutine(StopFireAfterDelay(area, 3f));
    }

    private IEnumerator StopFireAfterDelay(WheelArea area, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        area.StopOnFire();
    }
}
