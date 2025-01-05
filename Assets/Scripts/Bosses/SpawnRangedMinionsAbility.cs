using UnityEngine;

public class SpawnRangedMinionsAbility : MonoBehaviour
{
    [SerializeField] private GameObject rangedMinionPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int numberOfMinionsToSpawn = 3;

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
            Instantiate(rangedMinionPrefab, spawnPoints[i].position, Quaternion.identity);
        }
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