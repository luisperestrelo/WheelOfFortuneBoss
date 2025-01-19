using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnFatTentacleAbility : MonoBehaviour
{
    [SerializeField] private GameObject fatTentaclePrefab;
    [SerializeField] private Transform tentacleCenter;
    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private float angleOffset = 0f;
    [SerializeField] private float onContactDamage = 10f;
    [SerializeField] private float testAngleStep = 200f;

    private GameObject player;

    private void Start()
    {
        // Find the player's position
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            testAngleStep += 15f;
            SpawnFatTentacle180DegreesFromPlayer();
        }
    }

    public void SpawnFatTentacle(float angle)
    {
        Vector3 direction = Quaternion.Euler(0, 0, angle + angleOffset) * Vector3.left;
        Vector3 spawnPosition = tentacleCenter.position + direction * spawnDistance;

        GameObject fatTentacle = Instantiate(fatTentaclePrefab, spawnPosition, Quaternion.Euler(0, 0, angle + angleOffset));

        // Adjust flipping based on angle, only flips horizontally 
        AdjustFlipping(fatTentacle, angle);

        FatTentacle fatTentacleComponent = fatTentacle.GetComponentInChildren<FatTentacle>();
        if (fatTentacleComponent != null)
        {
            fatTentacleComponent.Initialize(onContactDamage);
        }

        Wall wall = fatTentacle.GetComponentInChildren<Wall>();
        if (wall != null)
        {
            wall.Initialize(angle - 20f, angle + 20f);
        }
    }

    public void SpawnFatTentacle180DegreesFromPlayer()
    {


        // Calculate the direction from the boss (tentacleCenter) to the player
        Vector3 toPlayer = player.transform.position - tentacleCenter.position;

        float currentAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        currentAngle += 180f;

        Vector3 spawnDirection = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0f);

        Vector3 spawnPosition = tentacleCenter.position + spawnDirection * spawnDistance;

        GameObject fatTentacle = Instantiate(fatTentaclePrefab, spawnPosition, Quaternion.Euler(0, 0, currentAngle + angleOffset));

        // Adjust flipping based on angle, only flips horizontally 
        AdjustFlipping(fatTentacle, currentAngle);

        FatTentacle fatTentacleComponent = fatTentacle.GetComponentInChildren<FatTentacle>();
        if (fatTentacleComponent != null)
        {
            fatTentacleComponent.Initialize(onContactDamage);
        }

        Wall wall = fatTentacle.GetComponentInChildren<Wall>();
        if (wall != null)
        {
            wall.Initialize(currentAngle - 20f, currentAngle + 20f);
        }
    }



    //quadrant stuff unneccesary, we are just doing horizontal flipping
    private void AdjustFlipping(GameObject tentacleRoot, float angle)
    {
        angle = (angle % 360 + 360) % 360;

        float adjustedAngle = (angle + angleOffset) % 360;

        // Determine quadrant. Unnecessary, not working, we are just doing horizontal flipping
        int quadrant = (int)(adjustedAngle / 90) % 4; // 0 = top-right, 1 = top-left, 2 = bottom-left, 3 = bottom-right

        switch (quadrant)
        {
            case 0: // Top-right (0-90)
                // No flipping needed
                tentacleRoot.transform.localScale = new Vector3(1, 1, 1);
                break;
            case 1: // Top-left (90-180)
                // Flip vertically
                tentacleRoot.transform.localScale = new Vector3(1, -1, 1);
                break;
            case 2: // Bottom-left (180-270)
                // Flip vertically and horizontally
                tentacleRoot.transform.localScale = new Vector3(1, -1, 1);
                break;
            case 3: // Bottom-right (270-360)
                // Flip horizontally
                tentacleRoot.transform.localScale = new Vector3(1, 1, 1);
                break;
        }
    }

}
