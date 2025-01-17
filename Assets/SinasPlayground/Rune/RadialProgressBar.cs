using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialProgressBar : MonoBehaviour
{
     private  Material progressBarMaterial; 
    [Range(0, 1)] public float progress = 0f; 

void Start(){
    progressBarMaterial = GetComponent<SpriteRenderer>().material;
}

     void Update()
    {
        var x = 360 - (progress * 360);
        progressBarMaterial.SetFloat("_Arc2", x);
    }

    public void SetProgress(float value)
    {
        progress = value;
    }
}
