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
    public static CameraMovement instance;
    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (instance != null)
            Destroy(gameObject);
        else 
            instance = this;
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

    /// <param name="intensity">The maximum number of units the camera should move per frame.</param>
    /// <param name="time">The number of seconds the camera should shake for.</param>
    public void ShakeCamera(float intensity, float time)
    {
        StartCoroutine(ShakeCameraRoutine(intensity, time));
    }

    private IEnumerator ShakeCameraRoutine(float intensity, float time)
    {
        float timeElapsed = 0;
        while(timeElapsed < time)
        {
            timeElapsed += Time.unscaledDeltaTime;
            transform.Translate(Vector3.right * Random.Range(-intensity, intensity));
            yield return new WaitForSecondsRealtime(0);
        }
    }
}
