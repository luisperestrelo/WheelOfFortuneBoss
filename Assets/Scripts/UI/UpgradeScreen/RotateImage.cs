using UnityEngine;
using UnityEngine.Serialization;

public class RotateImage : MonoBehaviour
{
    public float rotationSpeed = 50f;  
    public bool autoPlay = false;
    private bool isPlaying = false;


    void Start()
    {
        isPlaying = autoPlay;
    }
        
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