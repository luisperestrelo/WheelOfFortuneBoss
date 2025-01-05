using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for creating and destroying areas on the wheel.
/// </summary>
public class WheelBuilder : MonoBehaviour
{
    public static WheelBuilder instance;
    private void Start()
    {
        if (instance == null)
            DontDestroyOnLoad(this);
        else
            Destroy(gameObject);
    }

    [SerializeField]
    private int _radius = 5;
    /// <summary>
    /// Instantiates a new segment that follows the cursor, then waits for the player to click before locking in that segment.
    /// </summary>
    public IEnumerator PlaceAreaRoutine(WheelArea areaPrefab)
    {
        bool isPlacing = true;

        WheelArea area = Instantiate(areaPrefab, transform.position, Quaternion.identity, parent: transform);

        while(isPlacing)
        {
            yield return null;
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 towardCursor = (cursorPos - (Vector2)transform.position).normalized;
            Debug.Log(towardCursor);
            area.transform.position = (Vector2) transform.position + (towardCursor * _radius);
            area.transform.up = -(transform.position - area.transform.position).normalized;

            if (Input.GetMouseButtonDown(0))
                isPlacing = false;
        }
    }
}
