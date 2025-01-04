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
    private BossControllerSimple _targetPrefab;

    [SerializeField]
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

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
            BossControllerSimple target = Instantiate(_targetPrefab, pos, Quaternion.identity);
            target.player = player;
            yield return new WaitForSeconds(interval);
        }
    }
}
