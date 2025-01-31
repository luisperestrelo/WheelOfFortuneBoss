using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRendRealitySlice : MonoBehaviour
{
    private float playerRotationSpeed;
    private float playerRotationSpeedSlowed;
    private float desiredRotationSpeed;

    [SerializeField] private bool isFastRend = true;

    void Start()
    {
        //playerRotationSpeed = FindObjectOfType<PlayerSpinMovement>().MaxRotationSpeed;
        playerRotationSpeed = 140f; // because of the slow zone etc and I don't want to ensure this won't introduce bugs
        playerRotationSpeedSlowed = playerRotationSpeed * 0.5f;
        desiredRotationSpeed = 0;

        if (isFastRend)
        {
            desiredRotationSpeed = playerRotationSpeed;
        }
        else
        {
            desiredRotationSpeed = playerRotationSpeedSlowed;
        }
    }

    void Update()
    {
        RotateObject(desiredRotationSpeed);
    }

    private void RotateObject(float rotationSpeed)
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
