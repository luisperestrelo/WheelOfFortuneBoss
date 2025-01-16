using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float wallStartAngle = 45f;
    [SerializeField] private float wallEndAngle = 90f;
    [SerializeField] private bool drawGizmo = true;
    [SerializeField] private CircularPath circularPath;
    [SerializeField] private Vector3 center;
    [SerializeField] private float radius;


    private void Start()
    {
        circularPath = FindObjectOfType<CircularPath>();
        center = circularPath.transform.position;

    }

    public float WallStartAngle
    {
        get { return wallStartAngle; }
    }

    public float WallEndAngle
    {
        get { return wallEndAngle; }
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmo)
        {
            return;
        }


        float radius = circularPath.GetRadius() + 5f;

        Vector3 startPoint = center + new Vector3(
            Mathf.Cos(wallStartAngle * Mathf.Deg2Rad) * radius,
            Mathf.Sin(wallStartAngle * Mathf.Deg2Rad) * radius,
            0f
        );

        Vector3 endPoint = center + new Vector3(
            Mathf.Cos(wallEndAngle * Mathf.Deg2Rad) * radius,
            Mathf.Sin(wallEndAngle * Mathf.Deg2Rad) * radius,
            0f
        );

        Gizmos.color = Color.red;
        Gizmos.DrawLine(center, startPoint);
        Gizmos.DrawLine(center, endPoint);
    }
}
