using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPortals : MonoBehaviour
{
    public GameObject[] portals; // i wanna see it in the inspector

    void Awake()
    {
        GameObject parentObject = GameObject.Find("PortalPair");
        if (parentObject == null)
        {
            Debug.LogWarning("Missing 'PortalPair' GameObject in scene!");
            return;
        }

        Transform[] childTransforms = parentObject.GetComponentsInChildren<Transform>(true);
        List<GameObject> childObjects = new List<GameObject>();


        foreach (Transform t in childTransforms)
        {
            if (t != parentObject.transform)
            {
                childObjects.Add(t.gameObject);
            }
        }

        portals = childObjects.ToArray();

        DisablePortals();
    }

    public void EnablePortals()
    {
        foreach (GameObject portal in portals)
        {
            if (portal != null)
            {
                portal.SetActive(true);
            }
        }
    }

    public void DisablePortals()
    {
        foreach (GameObject portal in portals)
        {
            if (portal != null)
            {
                portal.SetActive(false);
            }
        }
    }

    public void TogglePortals()
    {
        foreach (GameObject portal in portals)
        {
            portal.SetActive(!portal.activeSelf);
        }
    }


    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            //EnablePortals();
            TogglePortals();
        }
#endif
    }

}


