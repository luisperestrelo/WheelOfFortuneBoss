using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;



    public void SetLifetime(float time)
    {
        lifetime = time;
        Destroy(gameObject, lifetime);
    }
}