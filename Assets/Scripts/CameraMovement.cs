using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float followCursorStrength = 1;
    [Tooltip("How far the cursor has to move from its anchor to reach the maximum following strength")]
    [SerializeField] private float distanceForMaxFollowStrength = 5;
    [SerializeField] private float zoomInStrength = 1;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private Vector3 anchor;

    private float originalSize;

    private Vector3 originalPos;
    private Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    private void Start()
    {
        originalSize = cam.orthographicSize;
        originalPos = transform.position;
    }
    private void LateUpdate()
    {
        LerpToPos(originalPos + TowardMouse() * followCursorStrength);
        UpdateSize();
    }

    private void UpdateSize()
    {
        float targetSize = originalSize;
        if (Input.GetMouseButton(0))
            targetSize -= zoomInStrength;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }

    private void LerpToPos(Vector3 pos)
    {
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed);
    }

    private Vector3 TowardMouse()
    {
        Plane plane = new(Vector3.forward, anchor);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hitPoint;

        float distance;
        plane.Raycast(ray, out distance);
        hitPoint = ray.GetPoint(distance);

        Vector3 target = hitPoint + anchor;
        target.z = 0;
        target /= distanceForMaxFollowStrength;

        if (target.magnitude > 1)
            target.Normalize();

        return target.normalized;
    }
}
