using UnityEngine;

/// <summary>
///   The CircularPath class defines the Player's circular path.
///   Other mechanics (such as boss abilities) can use this to target the player.   
/// </summary>
public class CircularPath : MonoBehaviour
{
    [SerializeField] private float radius = 5f;

    private void OnDrawGizmos()
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