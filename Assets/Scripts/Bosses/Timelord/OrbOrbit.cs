using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbOrbit : MonoBehaviour
{


    [SerializeField] private float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1) * rotationSpeed * Time.deltaTime);
    }
}
