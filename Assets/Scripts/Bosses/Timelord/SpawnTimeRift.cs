using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnTimeRift : MonoBehaviour
{
    [Header("Rift Prefab")]
    [Tooltip("The TimeRift prefab to spawn.")]
    [SerializeField] private GameObject timeRiftPrefab;

    [Header("Spawn Position")]
    [Tooltip("Relative or absolute position where the rift will be spawned.")]
    [SerializeField] private Vector3 spawnPosition = new Vector3(0f, 2.49f, 0f);

    [Header("Duration")]
    [Tooltip("How long the rift will exist before it is destroyed.")]
    [SerializeField] private float duration = 4f;



    void Start()
    {
        spawnPosition = new Vector3(0, 2.49f, 0);
    }

    // For testing/demo purposes, you can spawn a rift by pressing Space (or call this method from other code).
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SpawnTimeRiftAtRandomAngle();
        }
#endif

    }


    public void SpawnTimeRiftAtAngle(float angle)
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);


        GameObject rift = Instantiate(timeRiftPrefab, spawnPosition, rotation);
        
        Destroy(rift, duration);
    }


    public void SpawnTimeRiftAtRandomAngle()
    {
        float randomAngle = Random.Range(0f, 360f);
        SpawnTimeRiftAtAngle(randomAngle);
    }


}
