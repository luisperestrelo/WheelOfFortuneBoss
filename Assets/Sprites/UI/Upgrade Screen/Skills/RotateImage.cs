using UnityEngine;

public class RotateImage : MonoBehaviour
{
    public float rotationSpeed = 50f;  

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.unscaledDeltaTime);
    }
}