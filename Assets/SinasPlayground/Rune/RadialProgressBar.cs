using UnityEngine;

public class RadialProgressBar : MonoBehaviour
{
    [SerializeField] private Material baseMaterial; 
    
    private SpriteRenderer spriteRenderer; 
    private Material instanceMaterial; 

    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        instanceMaterial = new Material(baseMaterial);
        spriteRenderer.material = instanceMaterial;
        SetProgress(0f);
    }

    public void SetProgress(float value)
    {
        instanceMaterial?.SetFloat("_Arc2", 360 - (value * 360));  
    }
}