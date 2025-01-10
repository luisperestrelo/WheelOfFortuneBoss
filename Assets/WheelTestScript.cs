using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTestScript : MonoBehaviour
{
    [SerializeField] private WheelManager wheelManager;
    [SerializeField] private Field fieldToAdd;
    [SerializeField] private Field fieldToRemove;

    public void AddFieldToWheel()
    {
        wheelManager.AddField(fieldToAdd);
    }

    public void AddFieldToWheelAtIndex()
    {
        wheelManager.AddField(fieldToAdd, 3);
    }

    public void RemoveFieldFromWheel()
    {
        //wheelManager.RemoveField(fieldToRemove);
        wheelManager.RemoveField(0);
    }

    public void RemoveFieldFromWheelTemporarily()
    {
        wheelManager.ReplaceFieldTemporarily(0, fieldToAdd, 3f);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddFieldToWheel();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveFieldFromWheel();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddFieldToWheelAtIndex();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            RemoveFieldFromWheelTemporarily();
        }
    }
}
