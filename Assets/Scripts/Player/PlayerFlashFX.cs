using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashFX : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.1f;

    private Material originalMaterial;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    public void PlayFlashFX()
    {
        spriteRenderer.material = flashMaterial;
        Invoke("ResetMaterial", flashDuration);
    }



    private void ResetMaterial()
    {
        spriteRenderer.material = originalMaterial;
    }
}
