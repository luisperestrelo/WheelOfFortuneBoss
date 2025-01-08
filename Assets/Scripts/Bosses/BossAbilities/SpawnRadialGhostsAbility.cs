using UnityEngine;

public class SpawnRadialGhostsAbility : MonoBehaviour
{
    [SerializeField] private GameObject radialGhostPrefab;
    [SerializeField] private CircularPath path;
    [SerializeField] private Transform player;
    [SerializeField] private int numberOfGhosts = 2;

    [SerializeField] private float ghostSpeed = 2f;
    [SerializeField] private float ghostDamage = 10f;

    public void SpawnGhosts()
    {
        if (path == null)
        {
            Debug.LogError("CircularPath not assigned!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player Transform not assigned!");
            return;
        }

        //atm only makes sense to spawn 1 or 2 ghosts, but can later make a coroutine or something to spawn with an interval

        for (int i = 0; i < numberOfGhosts; i++)
        {
            GameObject ghost = Instantiate(radialGhostPrefab, path.GetCenter(), Quaternion.identity);
            int direction = i % 2 == 0 ? 1 : -1; // Alternate directions
            RadialGhost radialGhost = ghost.GetComponent<RadialGhost>();
            radialGhost.Initialize(path, player, direction, ghostSpeed, ghostDamage);
        }
    }
} 