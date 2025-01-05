using UnityEngine;
using System.Collections;

/// <summary>
/// This script is used to test the boss abilities manually, with inputs from the keyboard. 
/// </summary>
public class BossAbilityTester : MonoBehaviour
{
    public GameObject[] fields;
    public GameObject purpleSlashPrefab;
    public Transform player;
    public CircularSweepAttack circularSweepAttack;
    public SpawnRadialGhostsAbility spawnRadialGhostsAbility;
    public SpawnLinearGhostsAbility spawnLinearGhostsAbility;
    public SpawnChasingGhostAbility spawnChasingGhostAbility;
    public ThrowSpearsAbility throwSpearsAbility;




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeRandomFieldToFire();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            FireSlash();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            circularSweepAttack.StartCircularSweep();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            circularSweepAttack.StartCircularSweep(0f, false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawnRadialGhostsAbility.SpawnGhosts();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spawnLinearGhostsAbility.SpawnGhosts();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            spawnChasingGhostAbility.SpawnGhost();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            throwSpearsAbility.ThrowSpears();
        }
    }

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

    public void ChangeFieldToFire(int index)
    {
        if (index < 0 || index >= fields.Length)
        {
            return;
        }
        
        WheelArea area = fields[index].GetComponent<WheelArea>();
        area.SetOnFire();
        StartCoroutine(StopFireAfterDelay(area, 3f));
    }

    private IEnumerator StopFireAfterDelay(WheelArea area, float delay)
    {
        yield return new WaitForSeconds(delay);

        area.StopOnFire();
    }

    public void FireSlash()
    {
        GameObject purpleSlash = Instantiate(purpleSlashPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        purpleSlash.transform.rotation = Quaternion.Euler(0, 0, angle);
        purpleSlash.GetComponent<Rigidbody2D>().velocity = direction * 20f;
    }
}
