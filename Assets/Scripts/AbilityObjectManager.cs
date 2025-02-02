using System.Collections.Generic;
using UnityEngine;

public class AbilityObjectManager : MonoBehaviour
{
    public static AbilityObjectManager Instance { get; private set; }

    public List<GameObject> activeAbilityObjects = new List<GameObject>(); // i wanna see it in the inspector

    //singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Use this for all bosses, persist in scene, we'll get there when we have more
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterAbilityObject(GameObject obj)
    {
        if (!activeAbilityObjects.Contains(obj))
        {
            activeAbilityObjects.Add(obj);
        }
    }

    public void UnregisterAbilityObject(GameObject obj)
    {
        if (activeAbilityObjects.Contains(obj))
        {
            activeAbilityObjects.Remove(obj);
        }
    }

    public List<GameObject> GetActiveAbilityObjects()
    {
        return new List<GameObject>(activeAbilityObjects);
    }

    public void DestroyAllFatTentacles()
    {
        if (activeAbilityObjects == null || activeAbilityObjects.Count == 0) return;

        List<GameObject> objectsToDestroy = new List<GameObject>();
        foreach (GameObject obj in activeAbilityObjects)
        {
            if (obj.CompareTag("FatTentacle"))
            {
                objectsToDestroy.Add(obj);
            }
        }

        foreach (GameObject obj in objectsToDestroy)
        {
            UnregisterAbilityObject(obj);
            Destroy(obj);
        }
    }

    public void DestroyAllFlails()
    {
        if (activeAbilityObjects == null || activeAbilityObjects.Count == 0) return;

        List<GameObject> objectsToDestroy = new List<GameObject>();
        foreach (GameObject obj in activeAbilityObjects)
        {
            if (obj.CompareTag("Flail"))
            {
                objectsToDestroy.Add(obj);
            }
        }

        foreach (GameObject obj in objectsToDestroy)
        {
            UnregisterAbilityObject(obj);
            Destroy(obj);
        }
    }

    public void DestroyAllBigWavesOfDoom()
    {
        if (activeAbilityObjects == null || activeAbilityObjects.Count == 0) return;

        List<GameObject> objectsToDestroy = new List<GameObject>();
        foreach (GameObject obj in activeAbilityObjects)
        {
            if (obj.CompareTag("BigWaveOfDoom"))
            {
                objectsToDestroy.Add(obj);
            }
        }

        foreach (GameObject obj in objectsToDestroy)
        {
            UnregisterAbilityObject(obj);
            Destroy(obj);
        }
    }
    public void DestroyAllTelegraphs()
    {
        if (activeAbilityObjects == null || activeAbilityObjects.Count == 0) return;

        List<GameObject> objectsToDestroy = new List<GameObject>();
        foreach (GameObject obj in activeAbilityObjects)
        {
            if (obj.CompareTag("Telegraph"))
            {
                objectsToDestroy.Add(obj);
            }
        }

        foreach (GameObject obj in objectsToDestroy)
        {
            UnregisterAbilityObject(obj);
            Destroy(obj);
        }
        
        
    }

    public void DisableAllPortals()
    {
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
        foreach (GameObject portal in portals)
        {
            portal.SetActive(false);
            
        }
    }

    public void DisableAllRotatingOrbs()
    {
        GameObject orbHolder = GameObject.Find("Orb Holder");
        if (orbHolder != null)
        {
            orbHolder.SetActive(false);
        }
    }

} 