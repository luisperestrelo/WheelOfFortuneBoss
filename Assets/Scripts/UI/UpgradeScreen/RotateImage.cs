using UnityEngine;
using UnityEngine.Serialization;

public class RotateImage : MonoBehaviour
{
    public float rotationSpeed = 50f;  
    private bool isPlaying = false; 

        
    void Update()
    {
        if(isPlaying)
            transform.Rotate(0, 0, rotationSpeed * Time.unscaledDeltaTime);
    }

    public void Play()
    {
        isPlaying = true;
    }

    public void Stop()
    {
        isPlaying = false;
    }
    
    
    
}