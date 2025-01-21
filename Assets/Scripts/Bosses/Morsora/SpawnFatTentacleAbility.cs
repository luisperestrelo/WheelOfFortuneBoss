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

    // turns out, 180º degrees is not "the furthest away" from the player.
    // You would think, if we go further than 180º, then it is closer to the player if he changes the direction
    // but because there is acceleration in this game, changing direction doesnt actually get him there fater
    // So "furthest away" from player would be more something like 225º, depending on speed
    // by "furthest away" I mean, the angle that the player takes the longest ***time*** to reach from either direction
    public void SpawnFatTentacle180DegreesFromPlayer()
    {
        // Calculate the direction from the boss (tentacleCenter) to the player
        Vector3 toPlayer = player.transform.position - tentacleCenter.position;
        PlayerSpinMovement playerSpinMovement = player.GetComponent<PlayerSpinMovement>();

        float currentAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        // Adjust the angle based on the player's direction
        float directionAdjustment = playerSpinMovement.Direction > 0 ? 180f : -180f;
        currentAngle += directionAdjustment;

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

    public float SpawnFatTentacleDegreesFromPlayer(float angle)
    {
        // Calculate the direction from the boss (tentacleCenter) to the player
        Vector3 toPlayer = player.transform.position - tentacleCenter.position;
        PlayerSpinMovement playerSpinMovement = player.GetComponent<PlayerSpinMovement>();

        float currentAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        // Adjust the angle based on the player's direction and the input angle
        float directionAdjustment = playerSpinMovement.Direction > 0 ?   angle : -angle;
        currentAngle += directionAdjustment;

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



        return (currentAngle + 360f) % 360f;
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
