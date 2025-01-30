using UnityEngine;

public class LaserWithMovingGap : MonoBehaviour
{
    [SerializeField] private Transform topLaserTransform;
    [SerializeField] private Transform bottomLaserTransform;
    [SerializeField] private float gapHeight = 1f;
    [SerializeField] private float laserHeight = 5f;
    [SerializeField] private float moveSpeed = 1f;

    private float currentGapPosition = 0.5f; // Normalized position of the gap (0 to 1)
    private int moveDirection = 1; // 1 for up, -1 for down

    void Start()
    {
        

        UpdateLaserPositions();
    }

    void Update()
    {
        MoveGap();
        UpdateLaserPositions();
    }

    private void MoveGap()
    {
        currentGapPosition += moveSpeed * moveDirection * Time.deltaTime / laserHeight; // Normalize speed by laserHeight to control gap movement across the laser

        if (currentGapPosition > 1f)
        {
            currentGapPosition = 1f;
            moveDirection = -1;
        }
        else if (currentGapPosition < 0f)
        {
            currentGapPosition = 0f;
            moveDirection = 1;
        }
    }

    private void UpdateLaserPositions()
    {
        float gapCenterY = transform.localPosition.y + (currentGapPosition - 0.5f) * laserHeight; // Calculate gap center relative to laser parent
        float halfGapHeight = gapHeight / 2f;
        float halfLaserSectionHeight = (laserHeight - gapHeight) / 2f;

        if (topLaserTransform != null)
        {
            //topLaserTransform.localPosition = new Vector3(topLaserTransform.localPosition.x, gapCenterY + halfGapHeight + halfLaserSectionHeight, topLaserTransform.localPosition.z);
            topLaserTransform.localScale = new Vector3(topLaserTransform.localScale.x, halfLaserSectionHeight * 2f, topLaserTransform.localScale.z); // Scale to correct height
        }

        if (bottomLaserTransform != null)
        {
            //bottomLaserTransform.localPosition = new Vector3(bottomLaserTransform.localPosition.x, gapCenterY - halfGapHeight - halfLaserSectionHeight, bottomLaserTransform.localPosition.z);
            bottomLaserTransform.localScale = new Vector3(bottomLaserTransform.localScale.x, halfLaserSectionHeight * 2f, bottomLaserTransform.localScale.z); // Scale to correct height
        }
    }

    private void OnDrawGizmosSelected() // Optional: Draw gizmos in editor to visualize laser setup
    {
        Gizmos.color = Color.red;

        // Draw Laser bounds
        Vector3 laserCenter = transform.position;
        Gizmos.DrawWireCube(laserCenter + Vector3.up * (laserHeight / 2f - laserHeight / 2f), new Vector3(1f, laserHeight, 1f)); // Assuming width and depth are 1 for gizmo

        // Draw Gap bounds
        float gapCenterY = transform.position.y + (currentGapPosition - 0.5f) * laserHeight;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, gapCenterY, transform.position.z), new Vector3(1f, gapHeight, 1f));
    }
}
