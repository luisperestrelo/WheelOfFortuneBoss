using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flail : MonoBehaviour
{
    
    [SerializeField] private GameObject flailVfxPrefab;
    [SerializeField] private GameObject crackVfxPrefab;
    [SerializeField] private Transform flailVfxSpawnPosition;
    [SerializeField] private Transform crackVfxSpawnPosition;

    public void Hit()
    {
        Instantiate(flailVfxPrefab, flailVfxSpawnPosition.position, Quaternion.identity);
        Instantiate(crackVfxPrefab, crackVfxSpawnPosition.position, Quaternion.identity);
    }

}
