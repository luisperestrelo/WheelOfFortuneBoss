using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAnimation : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 1;

    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void OnCompleted()
    {
        if (col != null)
            col.enabled = false;
        StartCoroutine(DestroyAfterDelay());
    }
    
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
