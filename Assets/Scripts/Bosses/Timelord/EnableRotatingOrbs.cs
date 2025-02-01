using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRotatingOrbs : MonoBehaviour
{
    [SerializeField] private GameObject orbHolder;
    [SerializeField] private GameObject[] lasers;

    [SerializeField] private AudioSource attackSource;
    [SerializeField] private AudioClip spawnOrbsSfx;
    [SerializeField] private AudioClip activateLasersSfx;

    private void Start()
    {
        GameObject parentObj = GameObject.Find("TimelordSpawnPoints");
        if (parentObj == null)
        {
            Debug.LogWarning("TimelordSpawnPoints not found or not active.");
            return;
        }

        Transform childTransform = parentObj.transform.Find("Orb Holder");
        if (childTransform != null)
        {
            orbHolder = childTransform.gameObject;
            Debug.Log("Found orbHolder: " + orbHolder.name);

            List<GameObject> lasersList = new List<GameObject>();
            foreach (Transform orbTransform in orbHolder.transform)
            {
                Transform laserTransform = orbTransform.Find("Laser");
                if (laserTransform)
                {
                    lasersList.Add(laserTransform.gameObject);
                }
            }

            lasers = lasersList.ToArray();
        }
        else
        {
            Debug.LogWarning("Child 'Orb Holder' not found under TimelordSpawnPoints.");
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            EnableOrbHolder();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ToggleLasers(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ToggleLasers(false);
        }
#endif
    }

    public void EnableOrbHolder()
    {
        // Enable the orb holder
        orbHolder.SetActive(true);
        attackSource.PlayOneShot(spawnOrbsSfx);

        // Disable all lasers on each orb
        foreach (Transform orbTransform in orbHolder.transform)
        {
            Transform laserTransform = orbTransform.Find("Laser");
            if (laserTransform != null)
            {
                laserTransform.gameObject.SetActive(false);
            }
        }
    }

    public void ToggleLasers(bool enableLasers)
    {
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(enableLasers);
        }
        attackSource.PlayOneShot(activateLasersSfx);
        /*         foreach (Transform orbTransform in orbHolder.transform)
               {
                   Transform laserTransform = orbTransform.Find("Laser");

                   if (laserTransform != null)
                   {
                       laserTransform.gameObject.SetActive(enableLasers);
                   }
               } */
    }

    public void DisableOrbHolder()
    {
        orbHolder.SetActive(false);
    }
}
