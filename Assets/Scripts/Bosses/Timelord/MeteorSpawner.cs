using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private GameObject telegraphPrefab;
    [SerializeField] private GameObject impactVfxPrefab;
    [SerializeField] private CircularPath circularPath;

    [Header("Spawn Settings")]
    [Tooltip("How high above the ground the meteor starts when falling straight down.")]
    [SerializeField] private float spawnHeight = 10f;

    [Header("Diagonal Fall Control")]
    [Tooltip("If true, the meteor will spawn at a horizontal offset, creating a diagonal fall.")]
    [SerializeField] private bool isSpawningAtAnAngle = false;

    [Tooltip("Controls how far horizontally (XZ plane) the meteor is offset if spawning at an angle.")]
    [SerializeField] private Vector2 horizontalSpawnOffset = new Vector2(2f, 2f);

    [Tooltip("Rotation (Euler angles) applied to the meteor if spawning at an angle.")]
    [SerializeField] private Vector3 diagonalSpawnRotation = new Vector3(-30f, 0f, 0f);

    private void Start()
    {
        circularPath = FindObjectOfType<CircularPath>();
        if (circularPath == null)
        {
            Debug.LogError("CircularPath not found in the scene!");
        }
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnMeteorRandomAngle();
        }
        #endif
    }


    // --------------------------------------------------

    // Public Methods
    // --------------------------------------------------

    /// <summary>
    /// Spawns one meteor to land at the specified angle around the CircularPath.
    /// </summary>
    public void SpawnMeteorAtAngle(float angleInDegrees)
    {
        if (meteorPrefab == null || telegraphPrefab == null || impactVfxPrefab == null ||  circularPath == null)
        {
            Debug.LogWarning("MeteorSpawner: Missing a prefab or the circularPath reference!");
            return;
        }

        Vector3 center3D = circularPath.GetCenter();
        float radius = circularPath.GetRadius();


        float angleRad = angleInDegrees * Mathf.Deg2Rad;
        float x = center3D.x + radius * Mathf.Cos(angleRad);
        float y = center3D.y + radius * Mathf.Sin(angleRad);

        Vector3 landPosition = new Vector3(x, y, center3D.z); 

        // The 2D damage center (pure XY)
        Vector2 damageCenter = new Vector2(x, y);

        //  Where the meteor spawns (above the ground).
        Vector3 spawnPosition = landPosition + Vector3.up * spawnHeight;
        if (isSpawningAtAnAngle)
        {
            // Offset spawn horizontally
            spawnPosition += new Vector3(horizontalSpawnOffset.x, 0f, horizontalSpawnOffset.y);
        }

        GameObject telegraphObj = Instantiate(telegraphPrefab, landPosition, Quaternion.Euler(0, 0, 0));


        GameObject meteorObj = Instantiate(meteorPrefab, spawnPosition, Quaternion.Euler(-45, 0, 0));

        if (isSpawningAtAnAngle)
        {
            meteorObj.transform.rotation = Quaternion.Euler(diagonalSpawnRotation);
        }

        FallingMeteor fallingMeteor = meteorObj.GetComponent<FallingMeteor>();
        if (fallingMeteor != null)
        {
            // We pass "damageCenter" so OverlapCircle can use the correct 2D coords
            fallingMeteor.Init(landPosition, telegraphObj.transform, damageCenter, impactVfxPrefab);
        }
    }

    /// <summary>
    /// Spawns one meteor to land at a random angle around the CircularPath.
    /// </summary>
    public void SpawnMeteorRandomAngle()
    {
        float randomAngle = Random.Range(0f, 360f);
        SpawnMeteorAtAngle(randomAngle);
    }


}
