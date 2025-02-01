using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBigWaveOfDoom : MonoBehaviour
{
    [SerializeField] private GameObject _bigWaveOfDoomPrefab;
    [SerializeField] private Transform _spawnPoint;

    private void Start()
    {
        _spawnPoint = GameObject.Find("WaveOfDoomSpawnPoint").transform;

    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartWave();
        }
        #endif
    }
    public void StartWave()
    {
        GameObject bigWave = Instantiate(_bigWaveOfDoomPrefab, _spawnPoint.position, Quaternion.identity);
        AbilityObjectManager.Instance.RegisterAbilityObject(bigWave);
    }

}
