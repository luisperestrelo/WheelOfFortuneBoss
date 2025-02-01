using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deactivates all children of a gameobject immediately on start, then reactivates it a given number of seconds later. <br />
/// Used for the tutorial popups.
/// </summary>
public class ReactivateAfterDelay : MonoBehaviour
{
    [SerializeField] private float delay;
    private List<GameObject> children = new();

    private void Start()
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf)
            {
                children.Add(t.gameObject); //Add to list so we don't have to find inactive objects
                t.gameObject.SetActive(false);
            }
        }
        StartCoroutine(ReactivateAfterDelayRoutine());
    }

    private IEnumerator ReactivateAfterDelayRoutine()
    {
        yield return new WaitForSeconds(delay);
        foreach (GameObject o in children)
        {
            o.SetActive(true);
        }
    }
}
