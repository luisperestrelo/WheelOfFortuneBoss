using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTentacleSpitterAbility : MonoBehaviour
{
    [SerializeField] private GameObject tentacleSpitterPrefab;
    [SerializeField] private Transform tentacleCenter;
    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private float angleOffset = 0f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float shootingCooldown = 1f;

    [SerializeField] private float testAngleStep = 15f;


    [SerializeField] private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {


            SpawnTentacleSpitterAimedAtPlayer(5);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            testAngleStep += 15f;
            SpawnTentacleSpitter(testAngleStep, 1);

        }
    }

    public void SpawnTentacleSpitter(float angle, int numberOfSpits = 1)
    {
        Vector3 direction = Quaternion.Euler(0, 0, angle + angleOffset) * Vector3.left;
        Vector3 spawnPosition = tentacleCenter.position + direction * spawnDistance;

        GameObject tentacleSpitter = Instantiate(tentacleSpitterPrefab, spawnPosition, Quaternion.Euler(0, 0, angle + angleOffset));
        tentacleSpitter.GetComponent<LayerSort>()?.SortToBossLayer();
        // Adjust flipping based on angle, only flips horizontally 
        AdjustFlipping(tentacleSpitter, angle);

        TentacleSpitter tentacleSpitterComponent = tentacleSpitter.GetComponentInChildren<TentacleSpitter>();
        if (tentacleSpitterComponent != null)
        {
            tentacleSpitterComponent.Initialize(damage, numberOfSpits, projectileSpeed, shootingCooldown);
        }
    }

    public void SpawnTentacleSpitterAimedAtPlayer(int numberOfSpits = 1)
    {
        Vector3 direction = (player.transform.position - tentacleCenter.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        SpawnTentacleSpitter(angle, numberOfSpits);
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
