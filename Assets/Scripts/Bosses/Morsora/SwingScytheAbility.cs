using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Triggered by animation event
public class SwingScytheAbility : MonoBehaviour
{
    [SerializeField] private GameObject darkSlashPrefab;
    [SerializeField] private GameObject lightSlashPrefab;
    [SerializeField] private Transform darkSlashSpawnPoint;
    [SerializeField] private Transform lightSlashSpawnPoint;

    /*     private void Update()
        {

        }
        private void SwingScythe(bool isDarkSlash)
        {
            if (isDarkSlash)
            {
                Instantiate(darkSlashPrefab, darkSlashSpawnPoint.position, Quaternion.identity);
            }
            else
            {
                GameObject lightSlash = Instantiate(lightSlashPrefab, lightSlashSpawnPoint.position, Quaternion.identity);
                lightSlash.transform.rotation = Quaternion.Euler(0, 0, 180);

            }
        } */

    private void SwingLightScythe()
    {

        GameObject lightSlash = Instantiate(lightSlashPrefab, lightSlashSpawnPoint.position, Quaternion.identity);
        lightSlash.transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    private void SwingDarkScythe()
    {
        Instantiate(darkSlashPrefab, darkSlashSpawnPoint.position, Quaternion.identity);
    }

}
