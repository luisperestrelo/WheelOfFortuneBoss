using UnityEngine;

public class SpawnLinearGhostsAbility : MonoBehaviour
{
    [SerializeField] private GameObject linearGhostPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private int numberOfGhosts = 3;
    [SerializeField] private float spawnInterval = 0.5f;

    public void SpawnGhosts()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform not assigned!");
            return;
        }

        StartCoroutine(SpawnGhostsCoroutine());
    }

    private System.Collections.IEnumerator SpawnGhostsCoroutine()
    {
        for (int i = 0; i < numberOfGhosts; i++)
        {
            GameObject ghost = Instantiate(linearGhostPrefab, transform.position, Quaternion.identity);
            LinearGhost linearGhost = ghost.GetComponent<LinearGhost>();
            linearGhost.Initialize(player);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
} 