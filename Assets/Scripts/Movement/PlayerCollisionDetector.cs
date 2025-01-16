using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    public PlayerSpinMovement player;
    public float detectionDistance = 1f; // How far ahead to check for collisions
    public LayerMask obstacleLayer; // The layer your obstacles are on

    private void Update()
    {
        // Calculate the direction the player is moving
        Vector3 movementDirection = player.GetFuturePosition(Time.deltaTime) - player.transform.position;
        movementDirection.Normalize();

        // Cast a ray in the direction of movement
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, movementDirection, detectionDistance, obstacleLayer);

        if (hit.collider != null )
        {
            // Collision detected!
            //player.HandleCollision();
        }
    }

    private void OnDrawGizmos()
    {
        if (player == null)
        {
            return;
        }

        // Calculate the direction the player is moving
        Vector3 movementDirection = player.GetFuturePosition(Time.deltaTime) - player.transform.position;
        movementDirection.Normalize();

        Gizmos.color = Color.red;
        Gizmos.DrawRay(player.transform.position, movementDirection * detectionDistance);
    }
} 