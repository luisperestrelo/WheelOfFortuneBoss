using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowChakramAbility : MonoBehaviour
{
    [SerializeField] private GameObject chakramPrefab;
    [SerializeField] private Transform chakramSpawnPoint; // the hand without the staff/scythe
    [SerializeField] private PlayerSpinMovement playerMovement;
    [SerializeField] private float leadTime = 1f; // Time to lead the player by (in seconds). Set to 0 to not lead
    [SerializeField] private float speed = 10f;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerSpinMovement>();
    }



    public void ThrowChakramAtPredictedPlayerPosition()
    {
        // Get the predicted player position based on lead time
        Vector2 predictedPlayerPosition = playerMovement.GetFuturePosition(leadTime);

        GameObject chakram = Instantiate(chakramPrefab, chakramSpawnPoint.position, Quaternion.identity);

        Vector2 direction = (predictedPlayerPosition - (Vector2)chakramSpawnPoint.position).normalized;
        chakram.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    public void ThrowChakramAtPredictedPlayerPosition(float leadTime)
    {
        // Get the predicted player position based on lead time
        Vector2 predictedPlayerPosition = playerMovement.GetFuturePosition(leadTime);

        GameObject chakram = Instantiate(chakramPrefab, chakramSpawnPoint.position, Quaternion.identity);

        Vector2 direction = (predictedPlayerPosition - (Vector2)chakramSpawnPoint.position).normalized;
        chakram.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
