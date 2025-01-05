using UnityEngine;

public class SpawnChasingGhostAbility : MonoBehaviour
{
    [SerializeField] private GameObject chasingGhostPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] spawnPoints; // Where the ghost will spawn

    public void SpawnGhost()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform not assigned!");
            return;
        }

        if (spawnPoints == null)
        {
            Debug.LogError("Spawn Point not assigned!");
            return;
        }

        GameObject ghost = Instantiate(chasingGhostPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        ChasingGhost chasingGhost = ghost.GetComponent<ChasingGhost>();
        chasingGhost.Initialize(player);
    }
} 