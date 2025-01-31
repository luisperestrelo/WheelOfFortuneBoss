using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRotatingOrbs : MonoBehaviour
{
    [SerializeField] private GameObject orbHolder;
    [SerializeField] private GameObject[] lasers;


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
