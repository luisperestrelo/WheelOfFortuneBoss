using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Fix rotation
public enum SpawnDirection
{
    Clockwise,
    CounterClockwise
}

public class SpawnTentacleSnapAbility : MonoBehaviour
{
    [SerializeField] private GameObject tentacleSnapPrefab;
    [SerializeField] private GameObject tentacleSnapSlowerPrefab;
    [SerializeField] private Transform tentacleCenter;
    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private float angleOffset = 0f;
    [SerializeField] private float damage = 10f;

    [SerializeField] private Player player;

    [SerializeField] private float testAngleStep = 15f;

    private float cardinalRotationOffset = 0f;

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
    public void SpawnTentacleSnap(float angle, int numberOfSlams = 1, bool isSlower = false)
    {
        Vector3 direction = Quaternion.Euler(0, 0, angle + angleOffset) * Vector3.left;
        Vector3 spawnPosition = tentacleCenter.position + direction * spawnDistance;
        GameObject tentacleSnapRoot = Instantiate(isSlower ? tentacleSnapSlowerPrefab : tentacleSnapPrefab, spawnPosition, Quaternion.Euler(0, 0, angle + angleOffset));

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

    // For example, 3 tentacles would be 1 at player, 1 at 120 degrees, and 1 at 240 degrees    
    public void SpawnTentaclesAtAndAroundPlayer(int numberOfTentacles, int numberOfSlams = 1)
    {
        if (numberOfTentacles <= 0)
        {
            Debug.LogWarning("SpawnTentaclesAroundPlayer called with numberOfTentacles <= 0. No tentacles spawned.");
            return;
        }

        // Spawn one tentacle aimed at the player
        SpawnTentacleSnapAtPlayer(numberOfSlams);

        float angleStep = 360f / numberOfTentacles;

        for (int i = 1; i < numberOfTentacles; i++)
        {
            float angle = angleStep * i;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * (player.transform.position - tentacleCenter.position).normalized;
            float spawnAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            SpawnTentacleSnap(spawnAngle, numberOfSlams);
        }
    }

    public void SpawnTentaclesRandomlyAtAndAroundPlayer(int numberOfTentacles, int numberOfSlams = 1, float minAngleBetweenTentacles = 45f)
    {
        if (numberOfTentacles <= 0)
        {
            Debug.LogWarning("SpawnTentaclesRandomlyAroundPlayer called with numberOfTentacles <= 0. No tentacles spawned.");
            return;
        }

        // Spawn one tentacle aimed at the player
        SpawnTentacleSnapAtPlayer(numberOfSlams);

        // Keep track of the angles of the spawned tentacles
        List<float> spawnedAngles = new List<float>();

        // Spawn the remaining tentacles
        for (int i = 1; i < numberOfTentacles; i++)
        {
            float randomAngle;
            bool angleValid;

            do
            {
                randomAngle = Random.Range(0f, 360f);
                angleValid = true;

                foreach (float spawnedAngle in spawnedAngles)
                {
                    float angleDifference = Mathf.Abs(Mathf.DeltaAngle(randomAngle, spawnedAngle));
                    if (angleDifference < minAngleBetweenTentacles)
                    {
                        angleValid = false;
                        break;
                    }
                }
            } while (!angleValid);

            Vector3 direction = Quaternion.Euler(0, 0, randomAngle) * (player.transform.position - tentacleCenter.position).normalized;

            float spawnAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            SpawnTentacleSnap(spawnAngle, numberOfSlams);

            spawnedAngles.Add(randomAngle);
        }
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

    public void SpawnTentaclesAroundPlayer(int numberOfTentacles, int numberOfSlams = 1)
    {
        if (numberOfTentacles <= 0)
        {
            Debug.LogWarning("SpawnTentaclesAroundPlayer called with numberOfTentacles <= 0. No tentacles spawned.");
            return;
        }

        // Prevent spawning more tentacles than would fit
        if (numberOfTentacles > 360)
        {
            Debug.LogWarning($"Too many tentacles requested in SpawnTentaclesAroundPlayer. Reducing to maximum possible.");
            numberOfTentacles = 360;
        }

        // Spawn one tentacle aimed at the player
        SpawnTentacleSnapAtPlayer(numberOfSlams);

        float angleStep = 360f / numberOfTentacles;

        // Spawn the remaining tentacles
        for (int i = 1; i < numberOfTentacles; i++)
        {
            float angle = angleStep * i;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * (player.transform.position - tentacleCenter.position).normalized;
            float spawnAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            SpawnTentacleSnap(spawnAngle, numberOfSlams);
        }
    }

    public void SpawnTentaclesRandomlyAroundPlayer(int numberOfTentacles, int numberOfSlams = 1, float minAngleBetweenTentacles = 45f)
    {
        if (numberOfTentacles <= 0)
        {
            Debug.LogWarning("SpawnTentaclesRandomlyAroundPlayer called with numberOfTentacles <= 0. No tentacles spawned.");
            return;
        }

        // Check if it's even possible to spawn the requested number of tentacles with the given minimum angle
        if (360f / numberOfTentacles < minAngleBetweenTentacles)
        {
            Debug.LogWarning($"Cannot spawn {numberOfTentacles} tentacles with a minimum angle of {minAngleBetweenTentacles} degrees between them. Reducing the number of tentacles.");
            numberOfTentacles = Mathf.FloorToInt(360f / minAngleBetweenTentacles);
        }

        // Spawn one tentacle aimed at the player
        SpawnTentacleSnapAtPlayer(numberOfSlams);

        // Keep track of the angles of the spawned tentacles
        List<float> spawnedAngles = new List<float>();

        // Spawn the remaining tentacles
        for (int i = 1; i < numberOfTentacles; i++)
        {
            float randomAngle;
            bool angleValid;
            int attempts = 0;
            int maxAttempts = 100; // Prevent an infinite loop if valid angles are hard to find

            do
            {
                randomAngle = Random.Range(0f, 360f);
                angleValid = true;

                foreach (float spawnedAngle in spawnedAngles)
                {
                    float angleDifference = Mathf.Abs(Mathf.DeltaAngle(randomAngle, spawnedAngle));
                    if (angleDifference < minAngleBetweenTentacles)
                    {
                        angleValid = false;
                        break;
                    }
                }

                attempts++;
                if (attempts >= maxAttempts)
                {
                    Debug.LogWarning($"Could not find a valid angle for tentacle {i + 1} after {maxAttempts} attempts. Exiting loop.");
                    // Exit the loop early if maxAttempts is reached
                    goto ExitLoop;
                }

            } while (!angleValid);

            Vector3 direction = Quaternion.Euler(0, 0, randomAngle) * (player.transform.position - tentacleCenter.position).normalized;

            float spawnAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            SpawnTentacleSnap(spawnAngle, numberOfSlams);

            spawnedAngles.Add(randomAngle);
        }

    ExitLoop:; // Label for exiting the loop early
    }

    public void SpawnTentaclesAroundPlayerWithDelay(int numberOfTentacles, int numberOfSlams = 1, float delay = 0.5f)
    {
        StartCoroutine(SpawnTentaclesAroundPlayerWithDelayCoroutine(numberOfTentacles, numberOfSlams, delay));
    }

    private IEnumerator SpawnTentaclesAroundPlayerWithDelayCoroutine(int numberOfTentacles, int numberOfSlams, float delay)
    {
        if (numberOfTentacles <= 0)
        {
            Debug.LogWarning("SpawnTentaclesAroundPlayerWithDelay called with numberOfTentacles <= 0. No tentacles spawned.");
            yield break;
        }

        if (numberOfTentacles > 360)
        {
            Debug.LogWarning($"Too many tentacles requested in SpawnTentaclesAroundPlayerWithDelay. Reducing to maximum possible.");
            numberOfTentacles = 360;
        }

        // Spawn one tentacle aimed at the player
        SpawnTentacleSnapAtPlayer(numberOfSlams);
        yield return new WaitForSeconds(delay);

        // Calculate the angle step for the remaining tentacles
        float angleStep = 360f / numberOfTentacles;

        // Spawn the remaining tentacles
        for (int i = 1; i < numberOfTentacles; i++)
        {
            float angle = angleStep * i;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * (player.transform.position - tentacleCenter.position).normalized;
            float spawnAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            SpawnTentacleSnap(spawnAngle, numberOfSlams);

            yield return new WaitForSeconds(delay);
        }
    }

    public void SpawnTentaclesRandomlyAroundPlayerWithDelay(int numberOfTentacles, int numberOfSlams = 1, float minAngleBetweenTentacles = 45f, float delay = 0.5f)
    {
        StartCoroutine(SpawnTentaclesRandomlyAroundPlayerWithDelayCoroutine(numberOfTentacles, numberOfSlams, minAngleBetweenTentacles, delay));
    }

    private IEnumerator SpawnTentaclesRandomlyAroundPlayerWithDelayCoroutine(int numberOfTentacles, int numberOfSlams, float minAngleBetweenTentacles, float delay)
    {
        if (numberOfTentacles <= 0)
        {
            Debug.LogWarning("SpawnTentaclesRandomlyAroundPlayerWithDelay called with numberOfTentacles <= 0. No tentacles spawned.");
            yield break;
        }

        // Check if it's even possible to spawn the requested number of tentacles with the given minimum angle
        if (360f / numberOfTentacles < minAngleBetweenTentacles)
        {
            Debug.LogWarning($"Cannot spawn {numberOfTentacles} tentacles with a minimum angle of {minAngleBetweenTentacles} degrees between them. Reducing the number of tentacles.");
            numberOfTentacles = Mathf.FloorToInt(360f / minAngleBetweenTentacles);
        }

        // Keep track of the angles of the spawned tentacles
        List<float> spawnedAngles = new List<float>();

        // --- Calculate the angle to the player ---
        Vector3 directionToPlayer = (player.transform.position - tentacleCenter.position).normalized;
        float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        angleToPlayer = (angleToPlayer + 360) % 360;

        // Add the player's angle to the list of spawned angles
        spawnedAngles.Add(angleToPlayer);

        // Spawn one tentacle aimed at the player
        SpawnTentacleSnapAtPlayer(numberOfSlams);
        yield return new WaitForSeconds(delay);

        // Spawn the remaining tentacles
        for (int i = 1; i < numberOfTentacles; i++)
        {
            float randomAngle;
            bool angleValid;
            int attempts = 0;
            int maxAttempts = 100; // Prevent an infinite loop if valid angles are hard to find

            do
            {
                randomAngle = Random.Range(0f, 360f);
                angleValid = true;

                foreach (float spawnedAngle in spawnedAngles)
                {
                    float angleDifference = Mathf.Abs(Mathf.DeltaAngle(randomAngle, spawnedAngle));
                    if (angleDifference < minAngleBetweenTentacles)
                    {
                        angleValid = false;
                        break;
                    }
                }

                attempts++;
                if (attempts >= maxAttempts)
                {
                    Debug.LogWarning($"Could not find a valid angle for tentacle {i + 1} after {maxAttempts} attempts. Exiting loop.");
                    // Exit the loop early if maxAttempts is reached
                    yield break;
                }

            } while (!angleValid);

            float spawnAngle = (randomAngle + 360) % 360;

            spawnedAngles.Add(spawnAngle);

            SpawnTentacleSnap(spawnAngle, numberOfSlams);

            yield return new WaitForSeconds(delay);
        }
    }

    //Triggered by animation event
    public void SpawnTentaclesAtCardinals(int numberOfSlams = 1)
    {
        float[] angles = { 0, 90, 180, 270 };

        for (int i = 0; i < angles.Length; i++)
        {
            float spawnAngle = (angles[i] + cardinalRotationOffset) % 360;
            SpawnTentacleSnap(spawnAngle, numberOfSlams);
        }

        // Increment the rotation offset for the next call
        cardinalRotationOffset = (cardinalRotationOffset + 45f) % 360;
    }



    public void SpawnTentacleSpiral(float startingAngle, float delay, SpawnDirection direction, int numberOfSlams = 1)
    {
        StartCoroutine(SpawnTentacleSpiralCoroutine(startingAngle, delay, direction, numberOfSlams));
    }

    private IEnumerator SpawnTentacleSpiralCoroutine(float startingAngle, float delay, SpawnDirection direction, int numberOfSlams)
    {
        float angleStep = 15f;
        //int numberOfTentacles = 360 / (int)angleStep; // 24 tentacles total
        int numberOfTentacles = 540 / (int)angleStep; // 36 tentacles total

        for (int i = 0; i < numberOfTentacles; i++)
        {
            float currentAngle = startingAngle + (direction == SpawnDirection.Clockwise ?
                i * angleStep :
                -i * angleStep);

            // Normalize angle to 0-360 range
            currentAngle = (currentAngle % 360 + 360) % 360;

            SpawnTentacleSnap(currentAngle, numberOfSlams);
            yield return new WaitForSeconds(delay);
        }
    }

    public void SpawnCircleOfTentaclesWithGap(float gapCenterAngle, float gapSize, float angleStep = 15f, int numberOfSlams = 1)
    {

        gapCenterAngle = (gapCenterAngle % 360 + 360) % 360;

        float gapStart = (gapCenterAngle - gapSize/2 + 360) % 360;
        float gapEnd = (gapCenterAngle + gapSize/2 + 360) % 360;

        for (float angle = 0; angle < 360; angle += angleStep)
        {
            float normalizedAngle = (angle + 360) % 360;
            bool isInGap;
            
            if (gapStart <= gapEnd)
            {
                isInGap = normalizedAngle >= gapStart && normalizedAngle <= gapEnd;
            }
            else 
            {
                isInGap = normalizedAngle >= gapStart || normalizedAngle <= gapEnd;
            }

            if (!isInGap)
            {
                SpawnTentacleSnap(angle, numberOfSlams, true);
                //SpawnTentacleSnap(angle, numberOfSlams);
            }
        }
    }





}