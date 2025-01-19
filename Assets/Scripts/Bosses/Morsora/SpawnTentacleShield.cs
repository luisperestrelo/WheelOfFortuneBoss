using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is just visual, all logic is handled in the MorsoraBossController and MorsoraTentacleShieldState
public class SpawnTentacleShield : MonoBehaviour
{
    [SerializeField] private GameObject tentacleShield;
    [SerializeField] private List<GameObject> tentacles;
    [SerializeField] private float despawnDelay = 2f;
    [SerializeField] private Transform spawnSpot;

    private GameObject tentacleShieldInstance;

    private void Start()
    {
        tentacles = new List<GameObject>();
    }

    public void SpawnShield()
    {
        tentacleShieldInstance = Instantiate(tentacleShield, spawnSpot.position, spawnSpot.rotation);
        tentacles = new List<GameObject>();
        foreach (Transform child in tentacleShieldInstance.transform)
        {
            tentacles.Add(child.gameObject);
        }
    }

    public void DespawnShield()
    {
        foreach (GameObject tentacle in tentacles)
        {
            // play a despawn animation
            tentacle.GetComponent<Animator>().SetTrigger("Despawn");
        }
        Destroy(tentacleShieldInstance, despawnDelay);
    }
    }
