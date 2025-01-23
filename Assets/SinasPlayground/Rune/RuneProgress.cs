using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RuneProgress : MonoBehaviour
{

    [SerializeField] private Transform mask;
    [SerializeField]
    [Range(0, 1)] private float progress = 0f; 
    
    private Vector3 initialScale;

    void Awake(){
        initialScale = mask.localScale;
    }
    
    public void SetProgress(float value)
    {
        progress = value;
        mask.localScale = new Vector3(initialScale.x, value * initialScale.y, 1);
    }

    void Update() {
        mask.localScale = new Vector3(initialScale.x, progress * initialScale.y, 1);
    }

}
