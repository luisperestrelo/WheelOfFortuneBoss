using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private Transform[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        GameObject parentObject = GameObject.Find("BlackholeSpawnPoints");
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

    public void SpawnBlackhole()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(blackholePrefab, spawnPoints[randomIndex].position, Quaternion.Euler(-45, 0, 0));
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F11))
        {
            SpawnBlackhole();
        }
#endif
    }

}
