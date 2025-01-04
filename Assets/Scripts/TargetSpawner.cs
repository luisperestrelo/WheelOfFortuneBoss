using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField]
    private float interval = 2,
        xRange = 20,
        yRange = 15;

    [SerializeField]
    private GameObject _targetPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    /// <summary>
    /// Periodically spawns targets at a random position relative to this gameobject.
    /// </summary>
    private IEnumerator SpawnRoutine()
    {
        while(true)
        {
            Vector2 pos = new(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));
            Instantiate(_targetPrefab, pos, Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }
}
