using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRealityRend : MonoBehaviour
{
    [SerializeField] private GameObject realityRendFastPrefab;
    [SerializeField] private GameObject realityRendSlowPrefab;

    [SerializeField] private float realityRendDuration = 10f;
    [SerializeField] private AudioClip rendSfx;
    [SerializeField] private AudioSource source;

    private Vector3 spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = new Vector3(0, 2.49f, 0);
    }


    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SpawnRealityRendObject(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SpawnRealityRendObject(false);
        }
        #endif
        

    }


    public void SpawnRealityRendObject(bool isFastRend)
    {
        if (isFastRend)
        {
            GameObject realityRendFast = Instantiate(realityRendFastPrefab, spawnPosition, Quaternion.identity);
            Destroy(realityRendFast, realityRendDuration);
        }
        else

        {
            GameObject realityRendSlow = Instantiate(realityRendSlowPrefab, spawnPosition, Quaternion.identity);
            Destroy(realityRendSlow, realityRendDuration);
        }

        StartCoroutine(PlaySFXAfterDelay());

    }

    private IEnumerator PlaySFXAfterDelay()
    {
        yield return new WaitForSeconds(3.5f);
        source.PlayOneShot(rendSfx); //handled in this script and not the prefab so the tail of the sound doesnt get cut off when the prefab is destroyed
    }

    public float GetRealityRendDuration()
    {
        return realityRendDuration;
    }
}
