using System.Collections.Generic;
using UnityEngine;

public class AbilityObjectManager : MonoBehaviour
{
    public static AbilityObjectManager Instance { get; private set; }

    public List<GameObject> activeAbilityObjects = new List<GameObject>(); // i wanna see it in the inspector

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep the manager persistent across scenes
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
} 