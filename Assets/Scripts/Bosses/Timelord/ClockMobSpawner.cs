using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockMobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject clockMobPrefab;
    [SerializeField] private Transform[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        GameObject parentObject = GameObject.Find("ClockMobSpawnPoints");
        Debug.Log("Parent Object: " + parentObject);
        if (parentObject != null)
        {
            Transform[] children = parentObject.GetComponentsInChildren<Transform>();

            Debug.Log("Children Length: " + children.Length);

            for (int i = 1; i < children.Length; i++)
            {
                spawnPoints[i - 1] = children[i];
                Debug.Log("Spawn Point: " + spawnPoints[i - 1]);
            }
        }
    }

    public void SpawnClockMob()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(clockMobPrefab, spawnPoints[randomIndex].position, Quaternion.Euler(-45, 0, 0));
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnClockMob();
        }
#endif
    }

}
