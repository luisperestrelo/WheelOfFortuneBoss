using UnityEngine;

public class SpawnLinearGhostsAbility : MonoBehaviour
{
    [SerializeField] private GameObject linearGhostPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private int numberOfGhosts = 3;
    [SerializeField] private float spawnInterval = 0.5f;

    // New fields to control parameters from this script
    [SerializeField] private float ghostSpeed = 5f;
    [SerializeField] private float ghostDamage = 10f;
    [SerializeField] private float ghostLifeTime = 10f;

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

            // Set parameters on the linear ghost , i think it looks cleaner than initialize() but some refacor should be done
            linearGhost.SetSpeed(ghostSpeed);
            linearGhost.SetDamage(ghostDamage);
            linearGhost.SetLifeTime(ghostLifeTime);

            linearGhost.Initialize(player);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
} 