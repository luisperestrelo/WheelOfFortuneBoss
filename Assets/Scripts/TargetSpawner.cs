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
    private BossAbilityTester _targetPrefab;

    [SerializeField]
    private Transform player;

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
            BossAbilityTester target = Instantiate(_targetPrefab, pos, Quaternion.identity);
            target.player = player;
            yield return new WaitForSeconds(interval);
        }
    }
}
