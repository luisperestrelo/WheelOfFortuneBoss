using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingScytheAbility : MonoBehaviour
{
    [SerializeField] private GameObject darkSlashPrefab;
    [SerializeField] private GameObject lightSlashPrefab;
    [SerializeField] private Transform darkSlashSpawnPoint;
    [SerializeField] private Transform lightSlashSpawnPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SwingScythe(true);
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            SwingScythe(false);
        }
    }
    private void SwingScythe(bool isDarkSlash)
    {
        if (isDarkSlash)
        {
            Instantiate(darkSlashPrefab, darkSlashSpawnPoint.position, darkSlashSpawnPoint.rotation);
        }
        else
        {
            Instantiate(lightSlashPrefab, lightSlashSpawnPoint.position, lightSlashSpawnPoint.rotation);
        }

    }

}
