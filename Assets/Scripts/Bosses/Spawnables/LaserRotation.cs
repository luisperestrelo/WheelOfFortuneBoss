using UnityEngine;

public class LaserRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;
    private bool isClockwise = true;

    private void Update()
    {
        float speedMultiplier = isClockwise ? 1f : -1f;
        transform.Rotate(Vector3.forward, rotationSpeed * speedMultiplier * Time.deltaTime);
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    public void SetRotationDirection(bool isClockwise)
    {
        this.isClockwise = isClockwise;
    }
}