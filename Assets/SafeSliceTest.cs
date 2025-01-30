using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeSliceTest : MonoBehaviour
{
    private float playerRotationSpeed;
    private float playerRotationSpeedSlowed;
    private float desiredRotationSpeed;

    void Start()
    {
        playerRotationSpeed = FindObjectOfType<PlayerSpinMovement>().MaxRotationSpeed;
        playerRotationSpeedSlowed = playerRotationSpeed * 0.5f;
        desiredRotationSpeed = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Start rotating this object at the same speed as playerotationspeed
            desiredRotationSpeed = playerRotationSpeed;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Start rotating this object at the same speed as playerotationspeed
            desiredRotationSpeed = playerRotationSpeedSlowed;
        }

        RotateObject(desiredRotationSpeed);
        #endif
    }

    private void RotateObject(float rotationSpeed)
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
