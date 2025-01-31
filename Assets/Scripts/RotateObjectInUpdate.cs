using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectInUpdate : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
