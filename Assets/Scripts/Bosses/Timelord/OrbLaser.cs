using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbLaser : MonoBehaviour
{
    [SerializeField] private GameObject laserTelegraphPrefab;
    [SerializeField] private GameObject laserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTelegraph(float duration)
    {
        GameObject telegraph = Instantiate(laserTelegraphPrefab, transform.position, Quaternion.identity);
        Destroy(telegraph, duration);
    }

    public void SpawnLaser()
    {
        Instantiate(laserPrefab, transform.position, Quaternion.identity);
    }

    public void FireLaser(float telegraphDuration)
    {
        StartCoroutine(FireLaserRoutine(telegraphDuration));
    }

    private IEnumerator FireLaserRoutine(float telegraphDuration)
    {
        SpawnTelegraph(telegraphDuration);
        yield return new WaitForSeconds(telegraphDuration);
        SpawnLaser();
    }
}
