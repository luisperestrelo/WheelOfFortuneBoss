using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Fix rotation
public class SpawnTentacleSnapAbility : MonoBehaviour
{
    [SerializeField] private GameObject tentacleSnapPrefab;
    [SerializeField] private Transform tentacleCenter;
    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private float angleOffset = 0f;
    [SerializeField] private float damage = 10f;

    [SerializeField] private Player player;

    [SerializeField] private float testAngleStep = 15f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {


            SpawnTentacleSnapAtPlayer(5);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            SpawnTentacleSnap(testAngleStep, 1);
            testAngleStep += 15f;
        }
    }  

    //quadrant stuff unneccesary, we are just doing horizontal flipping
    public void SpawnTentacleSnap(float angle, int numberOfSlams = 1)
    {
        Vector3 direction = Quaternion.Euler(0, 0, angle + angleOffset) * Vector3.left;
        Vector3 spawnPosition = tentacleCenter.position + direction * spawnDistance;
        GameObject tentacleSnapRoot = Instantiate(tentacleSnapPrefab, spawnPosition, Quaternion.Euler(0, 0, angle + angleOffset));

        AdjustFlipping(tentacleSnapRoot, angle);

        TentacleSnap tentacleSnapComponent = tentacleSnapRoot.GetComponentInChildren<TentacleSnap>();
        if (tentacleSnapComponent != null)
        {
            tentacleSnapComponent.Initialize(damage, numberOfSlams);
        }
    }

    public void SpawnTentacleSnapAtPlayer(int numberOfSlams = 1)
    {
        Vector3 direction = (player.transform.position - tentacleCenter.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        SpawnTentacleSnap(angle, numberOfSlams);
    }

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