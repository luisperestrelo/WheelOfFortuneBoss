using System;
using UnityEngine;


public abstract class ClockMob : MonoBehaviour
{
    
    [SerializeField] protected Transform pointer;
    [SerializeField] private Color color = Color.red;
    [SerializeField] private SpriteRenderer progressImage;

    protected Action OnComplete;


    protected Material progressMat;
    // protected float currentDegree = 0f;
    protected bool isRunning = false;
    
    protected virtual void Awake()
    {
        isRunning = false;
        progressMat = new Material(progressImage.material);
        progressImage.material = progressMat;
        progressImage.color = color;
    }
    
    protected virtual void Start()
    {
        progressMat.SetFloat("_Arc2", 360);
    }
}