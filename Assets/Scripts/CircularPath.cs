using UnityEngine;

/// <summary>
///   The CircularPath class is used to define a circular path for an object to follow.
///   ie the player moves along this Path
///   Right now the player doesn't actually use this, but it's good for other objects to access the center and radius of the path
/// </summary>
public class CircularPath : MonoBehaviour
{
    [SerializeField] private float radius = 5f;


    // Optional: Draw the path in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public Vector3 GetCenter()
    {
        return transform.position;
    }

    public float GetRadius()
    {
        return radius;
    }
}