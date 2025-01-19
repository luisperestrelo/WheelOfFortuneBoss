using System.Collections;
using UnityEngine;

public class SpawnRangedMinionsAbility : MonoBehaviour
{
    [SerializeField] private GameObject rangedMinionPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int numberOfMinionsToSpawn = 3;

    [Tooltip("How long between minion spawns")]
    [SerializeField] private float desyncTime;

    // New fields to control parameters from this script
    [SerializeField] private float minionProjectileSpeed = 10f;
    [SerializeField] private float minionShootingCooldown = 1f;
    [SerializeField] private float minionDamage = 5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            SpawnMinions();
        }
    }

    public void SpawnMinions()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned for ranged minions!");
            return;
        }

        // Ensure we don't try to spawn more minions than there are spawn points
        int numToSpawn = Mathf.Min(numberOfMinionsToSpawn, spawnPoints.Length);

        // Shuffle the spawn points array to randomize spawn locations
        Shuffle(spawnPoints);

        for (int i = 0; i < numToSpawn; i++)
        {
            StartCoroutine(SpawnMinionAfterDelay(i * desyncTime, i));
        }
    }

    private IEnumerator SpawnMinionAfterDelay(float time, int spawnPointIndex)
    {
        yield return new WaitForSeconds(time);
        Vector3 spawnPosition = spawnPoints[spawnPointIndex].position;
        spawnPosition += Random.insideUnitSphere * 0.5f; // Add a small random offset
        GameObject minion = Instantiate(rangedMinionPrefab, spawnPosition, Quaternion.identity);
        RangedMinion rangedMinion = minion.GetComponent<RangedMinion>();
        rangedMinion.Initialize(minionProjectileSpeed, minionShootingCooldown, minionDamage);
    }

    // Fisher-Yates shuffle algorithm
    private void Shuffle(Transform[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
} 