using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCameraOnStart : MonoBehaviour
{
    [SerializeField] private float intensity, time;

    private void Start()
    {
        CameraMovement.instance.ShakeCamera(intensity, time);
    }
}
