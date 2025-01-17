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



    public void Initialize(float wallStartAngle, float wallEndAngle)
    {
        this.wallStartAngle = wallStartAngle;
        this.wallEndAngle = wallEndAngle;
        // use modulo to wrap around 360, not allowing negatives
        this.wallStartAngle = (wallStartAngle + 360) % 360;
        this.wallEndAngle = (wallEndAngle + 360) % 360;

    }

    private void Start()
    {
        circularPath = FindObjectOfType<CircularPath>();
        center = GameObject.FindGameObjectWithTag("MorsoraTentacleCenter").transform.position;
        //center = circularPath.transform.position;

    }

    private void Update()
    {

    }

    public void UpdateAngles(float newStartAngle, float newEndAngle)
    {
        wallStartAngle = newStartAngle;
        wallEndAngle = newEndAngle;

        // use modulo to wrap around 360, not allowing negatives
        wallStartAngle = (wallStartAngle + 360) % 360;
        wallEndAngle = (wallEndAngle + 360) % 360;
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


        //float radius = circularPath.GetRadius() + 15f;
        float radius = 15f;

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
